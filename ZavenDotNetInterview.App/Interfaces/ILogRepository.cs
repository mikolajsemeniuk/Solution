using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZavenDotNetInterview.App.Models;

namespace ZavenDotNetInterview.App.Interfaces
{
    public interface ILogRepository
    {
        Log CreateNewLog(string description, Guid jobId);
        Task<IEnumerable<Log>> GetLogsByIdOrderedByCreatedDate(Guid jobId);
    }
}
