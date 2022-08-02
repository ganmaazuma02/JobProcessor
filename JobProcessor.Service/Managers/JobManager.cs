using JobProcessor.Data;
using JobProcessor.Data.EntityModels;
using JobProcessor.Data.Enums;
using JobProcessor.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JobProcessor.Service.Managers
{
    public class JobManager : IJobManager
    {
        private readonly DataContext _dataContext;

        public JobManager(IServiceProvider serviceProvider)
        {
            _dataContext = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
        }

        public async Task EnqueueJobAsync(Job job)
        {
            await _dataContext.Jobs.AddAsync(job);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Job> GetJobAsync(string jobId)
        {
            var _guidJobId = Guid.Parse(jobId);
            return await _dataContext.Jobs.FindAsync(_guidJobId);
        }

        public async Task<IEnumerable<Job>> GetJobsAsync()
        {
            return await _dataContext.Jobs.ToListAsync();
        }

        public async Task CommitDatabaseChangesAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Job> GetEarliestQueuedJobAsync()
        {
            return await _dataContext.Jobs
                .Where(job => job.JobStatus == JobStatus.Queued)
                .OrderBy(job => job.JobEnqueuedDateTimeUtc)
                .FirstOrDefaultAsync();
        }
    }
}
