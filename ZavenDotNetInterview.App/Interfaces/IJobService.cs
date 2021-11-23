using System;
using System.Threading.Tasks;

namespace ZavenDotNetInterview.App.Interfaces
{
    public interface IJobService
    {
        Task<bool> CreateNewJobAsync(string name, DateTime? doAfter);
    }
}
