using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZavenDotNetInterview.App.Enums;

namespace ZavenDotNetInterview.App.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Name { get; set; }
        public JobStatus Status { get; set; }
        public DateTime? DoAfter { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public long FailureCounter { get; set; } = 0;
        public virtual IEnumerable<Log> Logs { get; set; }

        public void ChangeStatus(JobStatus status)
        {
            Status = status;
        }

        public void IncrementFailureCounterBy1()
        {
            FailureCounter += 1;
        }

        public void UpdateLastUpdatedAtToNow()
        {
            LastUpdatedAt = DateTime.Now;
        }
    }
}
