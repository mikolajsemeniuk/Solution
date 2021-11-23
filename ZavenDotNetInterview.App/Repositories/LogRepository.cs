using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ZavenDotNetInterview.App.DataContext;
using ZavenDotNetInterview.App.Interfaces;
using ZavenDotNetInterview.App.Models;

namespace ZavenDotNetInterview.App.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly ZavenDotNetInterviewContext _context;

        public LogRepository(ZavenDotNetInterviewContext context)
        {
            _context = context;
        }

        public Log CreateNewLog(string description, Guid jobId)
        {
            var log = new Log { Id = Guid.NewGuid(), Description = description, CreatedAt = DateTime.Now, JobId = jobId };
            _context.Logs.Add(log);
            return log;
        }

        public async Task<IEnumerable<Log>> GetLogsByIdOrderedByCreatedDate(Guid jobId)
        {
            return await _context.Logs.Where(log => log.JobId == jobId).OrderBy(log => log.CreatedAt).ToListAsync();
        }
    }
}