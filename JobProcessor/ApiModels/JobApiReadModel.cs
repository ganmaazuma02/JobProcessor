namespace JobProcessor.API.ApiModels
{
    public class JobApiReadModel
    {
        public Guid JobId { get; set; }
        public DateTime JobEnqueuedDateTimeUtc { get; set; }
        public long? JobProcessingDurationMiliseconds { get; set; }
        public string JobStatus { get; set; }
        public IEnumerable<int> JobInput { get; set; }
        public IEnumerable<int> JobOutput { get; set; }
    }
}
