using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZavenDotNetInterview.App.Models;

namespace ZavenDotNetInterview.App.Interfaces
{
    public interface IJobRepository
    {
        Task<bool> CheckIfJobWithNameExistsAsync(string name);
        Job CreateNewJob(string name, DateTime? doAfter);
        Task<Job> GetJobByIdAsync(Guid id);
        Task<IEnumerable<Job>> GetAllJobsSortedByDateAsync();
    }
}