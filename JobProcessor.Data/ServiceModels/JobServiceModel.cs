using JobProcessor.Data.Enums;

namespace JobProcessor.Data.ServiceModels
{
    public class JobServiceModel
    {
        public Guid JobId { get; set; }
        public DateTime JobEnqueuedDateTimeUtc { get; set; }
        public long? JobProcessingDurationMiliseconds { get; set; }
        public JobStatus JobStatus { get; set; }
        public IEnumerable<int>? JobInput { get; set; }
        public IEnumerable<int>? JobOutput { get; set; }
    }
}
