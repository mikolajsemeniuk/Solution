using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ZavenDotNetInterview.App.DataContext;
using ZavenDotNetInterview.App.Enums;
using ZavenDotNetInterview.App.Interfaces;
using ZavenDotNetInterview.App.Models;

namespace ZavenDotNetInterview.App.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly ZavenDotNetInterviewContext _context;

        public JobRepository(ZavenDotNetInterviewContext context)
        {
            _context = context;
        }

        public Job CreateNewJob(string name, DateTime? doAfter)
        {
            var job = new Job { Id = Guid.NewGuid(), Name = name, DoAfter = doAfter, Status = JobStatus.New };
            _context.Jobs.Add(job);
            return job;
        }

        public async Task<Job> GetJobByIdAsync(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job is null)
            {
                job = new Job { Id = Guid.Empty };
            }
            return job;
        }

        public async Task<IEnumerable<Job>> GetAllJobsSortedByDateAsync()
        {
            return await _context.Jobs.OrderBy(job => job.CreatedAt).ToListAsync();
        }

        public async Task<bool> CheckIfJobWithNameExistsAsync(string name)
        {
            var currentJob = await _context.Jobs.Where(job => job.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
            if (currentJob is null)
            {
                return false;
            }
            return true;
        }
    }
}