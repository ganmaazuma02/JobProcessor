using JobProcessor.Data;
using JobProcessor.Data.EntityModels;
using JobProcessor.Data.Enums;
using JobProcessor.Service.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace JobProcessor.UnitTests
{
    public class JobManagerTests
    {
        [Fact]
        public async Task EnqueueJobAsync_Should_AddOneJobToTheDbContext()
        {
            var _dbName = nameof(EnqueueJobAsync_Should_AddOneJobToTheDbContext);
            var _serviceCollection = new ServiceCollection();
            _serviceCollection.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            var _serviceProvider = _serviceCollection.BuildServiceProvider();
            var _mockLogger = new Mock<ILogger<JobManager>>();
            var _jobManager = new JobManager(_serviceProvider, _mockLogger.Object);

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(_dbName)
                .Options;

            using (var _context = new DataContext(options))
            {
                _context.Jobs.Add(new Job
                {
                    JobId = Guid.NewGuid(),
                    JobStatus = JobStatus.Queued,
                    JobProcessingDurationMiliseconds = 0,
                    JobEnqueuedDateTimeUtc = DateTime.UtcNow,
                    JobInput = "8,5,3,9,2,76",
                    JobOutput = string.Empty
                });

                _context.SaveChanges();
            }

            await _jobManager.EnqueueJobAsync(new Job
            {
                JobId = Guid.NewGuid(),
                JobStatus = JobStatus.Queued,
                JobProcessingDurationMiliseconds = 0,
                JobEnqueuedDateTimeUtc = DateTime.UtcNow,
                JobInput = "7,4,2,5,76,3",
                JobOutput = string.Empty
            });

            using (var _context = new DataContext(options))
            {
                Assert.Equal(2, _context.Jobs.Count());
            }

        }

        [Fact]
        public async Task GetJobsAsync_Should_ReturnAllJobs()
        {
            var _dbName = nameof(GetJobsAsync_Should_ReturnAllJobs);
            var _serviceCollection = new ServiceCollection();
            _serviceCollection.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            var _serviceProvider = _serviceCollection.BuildServiceProvider();
            var _mockLogger = new Mock<ILogger<JobManager>>();
            var _jobManager = new JobManager(_serviceProvider, _mockLogger.Object);

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(_dbName)
                .Options;

            using (var _context = new DataContext(options))
            {
                _context.Jobs.Add(new Job { 
                    JobId = Guid.NewGuid(),
                    JobStatus = JobStatus.Queued,
                    JobProcessingDurationMiliseconds = 0,
                    JobEnqueuedDateTimeUtc = DateTime.UtcNow,
                    JobInput = "8,5,3,9,2,76",
                    JobOutput = string.Empty
                });

                _context.Jobs.Add(new Job
                {
                    JobId = Guid.NewGuid(),
                    JobStatus = JobStatus.Queued,
                    JobProcessingDurationMiliseconds = 0,
                    JobEnqueuedDateTimeUtc = DateTime.UtcNow,
                    JobInput = "7,4,2,5,76,3",
                    JobOutput = string.Empty
                });

                _context.Jobs.Add(new Job
                {
                    JobId = Guid.NewGuid(),
                    JobStatus = JobStatus.Queued,
                    JobProcessingDurationMiliseconds = 0,
                    JobEnqueuedDateTimeUtc = DateTime.UtcNow,
                    JobInput = "2,5,7,8,90,4",
                    JobOutput = string.Empty
                });

                _context.SaveChanges();
            }

            var _jobs = await _jobManager.GetJobsAsync();
            Assert.Equal(3, _jobs.Count());

        }

        [Fact]
        public async Task GetJobAsync_Should_ReturnOneJobWithCorrectId()
        {
            var _dbName = nameof(GetJobAsync_Should_ReturnOneJobWithCorrectId);
            var _serviceCollection = new ServiceCollection();
            _serviceCollection.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            var _serviceProvider = _serviceCollection.BuildServiceProvider();
            var _mockLogger = new Mock<ILogger<JobManager>>();
            var _jobManager = new JobManager(_serviceProvider, _mockLogger.Object);

            var _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(_dbName)
                .Options;

            var _newGuid = Guid.NewGuid();

            using (var _context = new DataContext(_options))
            {
                _context.Jobs.Add(new Job
                {
                    JobId = _newGuid,
                    JobStatus = JobStatus.Queued,
                    JobProcessingDurationMiliseconds = 0,
                    JobEnqueuedDateTimeUtc = DateTime.UtcNow,
                    JobInput = "8,5,3,9,2,76",
                    JobOutput = string.Empty
                });

                _context.Jobs.Add(new Job
                {
                    JobId = Guid.NewGuid(),
                    JobStatus = JobStatus.Queued,
                    JobProcessingDurationMiliseconds = 0,
                    JobEnqueuedDateTimeUtc = DateTime.UtcNow,
                    JobInput = "7,4,2,5,76,3",
                    JobOutput = string.Empty
                });

                _context.Jobs.Add(new Job
                {
                    JobId = Guid.NewGuid(),
                    JobStatus = JobStatus.Queued,
                    JobProcessingDurationMiliseconds = 0,
                    JobEnqueuedDateTimeUtc = DateTime.UtcNow,
                    JobInput = "2,5,7,8,90,4",
                    JobOutput = string.Empty
                });

                _context.SaveChanges();
            }

            var _job = await _jobManager.GetJobAsync(_newGuid.ToString());
            Assert.Equal(_newGuid, _job.JobId);
        }

        [Fact]
        public async Task GetEarliestQueuedJobAsync_Should_ReturnEarliestQueuedJob()
        {
            var _dbName = nameof(GetEarliestQueuedJobAsync_Should_ReturnEarliestQueuedJob);
            var _serviceCollection = new ServiceCollection();
            _serviceCollection.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            var _serviceProvider = _serviceCollection.BuildServiceProvider();
            var _mockLogger = new Mock<ILogger<JobManager>>();
            var _jobManager = new JobManager(_serviceProvider, _mockLogger.Object);

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(_dbName)
                .Options;

            var _newGuid = Guid.NewGuid();

            using (var _context = new DataContext(options))
            {
                _context.Jobs.Add(new Job
                {
                    JobId = Guid.NewGuid(),
                    JobStatus = JobStatus.Queued,
                    JobProcessingDurationMiliseconds = 0,
                    JobEnqueuedDateTimeUtc = new DateTime(2022, 6, 1),
                    JobInput = "8,5,3,9,2,76",
                    JobOutput = string.Empty
                });

                _context.Jobs.Add(new Job
                {
                    JobId = Guid.NewGuid(),
                    JobStatus = JobStatus.Queued,
                    JobProcessingDurationMiliseconds = 0,
                    JobEnqueuedDateTimeUtc = new DateTime(2022, 5, 1),
                    JobInput = "7,4,2,5,76,3",
                    JobOutput = string.Empty
                });

                // earliest queued
                _context.Jobs.Add(new Job
                {
                    JobId = _newGuid,
                    JobStatus = JobStatus.Queued,
                    JobProcessingDurationMiliseconds = 0,
                    JobEnqueuedDateTimeUtc = new DateTime(2022, 4, 1),
                    JobInput = "2,5,7,8,90,4",
                    JobOutput = string.Empty
                });

                // earlier but processing
                _context.Jobs.Add(new Job
                {
                    JobId = Guid.NewGuid(),
                    JobStatus = JobStatus.Processing,
                    JobProcessingDurationMiliseconds = 0,
                    JobEnqueuedDateTimeUtc = new DateTime(2022, 3, 1),
                    JobInput = "2,5,7,8,90,4",
                    JobOutput = string.Empty
                });

                // earlier but completed
                _context.Jobs.Add(new Job
                {
                    JobId = Guid.NewGuid(),
                    JobStatus = JobStatus.Completed,
                    JobProcessingDurationMiliseconds = 0,
                    JobEnqueuedDateTimeUtc = new DateTime(2022, 2, 1),
                    JobInput = "2,5,7,8,90,4",
                    JobOutput = "2,4,5,7,8,90"
                });

                _context.SaveChanges();
            }

            var _job = await _jobManager.GetEarliestQueuedJobAsync();
            Assert.Equal(_newGuid, _job.JobId);

        }

    }
}
