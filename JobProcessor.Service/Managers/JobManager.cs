using JobProcessor.Data;
using JobProcessor.Data.EntityModels;
using JobProcessor.Data.Enums;
using JobProcessor.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JobProcessor.Service.Managers
{
    public class JobManager : IJobManager
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<JobManager> _logger;

        public JobManager(IServiceProvider serviceProvider, ILogger<JobManager> logger)
        {
            _dataContext = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
            _logger = logger;   
        }

        public async Task EnqueueJobAsync(Job job)
        {
            try
            {
                await _dataContext.Jobs.AddAsync(job);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error enqueueing a job. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<Job> GetJobAsync(string jobId)
        {
            try
            {
                var _guidJobId = Guid.Parse(jobId);
                return await _dataContext.Jobs.FindAsync(_guidJobId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting a job. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Job>> GetJobsAsync()
        {
            try
            {
                return await _dataContext.Jobs.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all jobs. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task CommitDatabaseChangesAsync()
        {
            try
            {
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error committing changes to the database. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<Job> GetEarliestQueuedJobAsync()
        {
            try
            {
                return await _dataContext.Jobs
                .Where(job => job.JobStatus == JobStatus.Queued)
                .OrderBy(job => job.JobEnqueuedDateTimeUtc)
                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting the earliest queued job. Exception message: {ex.Message}");
                throw;
            }
        }
    }
}
