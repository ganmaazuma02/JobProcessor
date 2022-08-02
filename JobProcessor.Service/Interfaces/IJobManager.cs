using JobProcessor.Data.EntityModels;

namespace JobProcessor.Service.Interfaces
{
    public interface IJobManager
    {
        Task EnqueueJobAsync(Job job);
        Task<Job> GetJobAsync(string jobId);
        Task<IEnumerable<Job>> GetJobsAsync();
        Task CommitDatabaseChangesAsync();
        Task<Job> GetEarliestQueuedJobAsync();
    }
}
