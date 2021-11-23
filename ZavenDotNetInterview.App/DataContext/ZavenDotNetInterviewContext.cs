using System.Data.Entity;
using ZavenDotNetInterview.App.Models;

//namespace ZavenDotNetInterview.App.Models.Context
namespace ZavenDotNetInterview.App.DataContext
{
    public class ZavenDotNetInterviewContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Log> Logs { get; set; }

        public ZavenDotNetInterviewContext() : base("name=ZavenDotNetInterview")
        {
        }
    }
}