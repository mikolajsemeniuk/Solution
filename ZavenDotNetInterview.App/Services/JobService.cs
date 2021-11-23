using System;
using System.Threading.Tasks;
using ZavenDotNetInterview.App.DataContext;
using ZavenDotNetInterview.App.Interfaces;

namespace ZavenDotNetInterview.App.Services
{
    public class JobService : IJobService
    {
        private readonly ZavenDotNetInterviewContext _context;
        private readonly IJobRepository _jobRepository;
        private readonly ILogRepository _logRepository;

        public JobService(ZavenDotNetInterviewContext context, IJobRepository jobRepository, ILogRepository logRepository)
        {
            _context = context;
            _jobRepository = jobRepository;
            _logRepository = logRepository;
        }
        public async Task<bool> CreateNewJobAsync(string name, DateTime? doAfter)
        {
            var job = _jobRepository.CreateNewJob(name, doAfter);
            _logRepository.CreateNewLog("New", job.Id);
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
