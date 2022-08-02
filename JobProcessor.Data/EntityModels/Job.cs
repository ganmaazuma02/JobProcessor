using JobProcessor.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobProcessor.Data.EntityModels
{
    public class Job
    {
        [Key]
        public Guid JobId { get; set; }
        public DateTime JobEnqueuedDateTimeUtc { get; set; }
        public long? JobProcessingDurationMiliseconds { get; set; }
        public JobStatus JobStatus { get; set; }
        public string? JobInput { get; set; }
        public string? JobOutput { get; set; }
    }
}
