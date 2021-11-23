using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ZavenDotNetInterview.App.DataContext;
using ZavenDotNetInterview.App.Interfaces;
using ZavenDotNetInterview.App.Models;

namespace ZavenDotNetInterview.App.Services
{
    public class JobProcessorService : IJobProcessorService
    {
        private readonly ZavenDotNetInterviewContext _context;
        private readonly ILogRepository _logRepository;

        public JobProcessorService(ZavenDotNetInterviewContext context, ILogRepository logRepository)
        {
            _context = context;
            _logRepository = logRepository;
        }

        private async Task<List<Job>> ChangeStatusToInProgress()
        {
            var jobsToProcess = await _context.Jobs
                .Where(job => job.Status == JobStatus.New && job.DoAfter == null ||
                       job.Status == JobStatus.New && job.DoAfter != null && job.DoAfter <= DateTime.Now ||
                       job.Status == JobStatus.Failed)
                .ToListAsync();

            jobsToProcess.ForEach(job =>
            {
                job.ChangeStatus(JobStatus.InProgress);
                job.UpdateLastUpdatedAtToNow();
                _logRepository.CreateNewLog("In Progress", job.Id);
            });

            await _context.SaveChangesAsync();
            return jobsToProcess;

        }

        public async Task ProcessJobs()
        {
            var jobsToProcess = await ChangeStatusToInProgress();
            var jobs = jobsToProcess.Select(async job =>
            {
                if (await WasJobSuccessful(job))
                {
                    job.ChangeStatus(JobStatus.Done);
                    _logRepository.CreateNewLog("Done", job.Id);
                }
                else
                {
                    job.IncrementFailureCounterBy1();
                    if (job.FailureCounter == 5)
                    {
                        job.ChangeStatus(JobStatus.Closed);
                        _logRepository.CreateNewLog("Closed", job.Id);
                    }
                    else
                    {
                        job.ChangeStatus(JobStatus.Failed);
                        _logRepository.CreateNewLog("Failed", job.Id);
                    }
                }
                job.UpdateLastUpdatedAtToNow();
            });
            await Task.WhenAll(jobs);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> WasJobSuccessful(Job job)
        {
            Random rand = new Random();
            if (rand.Next(10) < 5)
            {
                await Task.Delay(2000);
                return false;
            }
            else
            {
                await Task.Delay(1000);
                return true;
            }
        }
    }
}