using AutoMapper;
using JobProcessor.API.MapperProfiles;
using JobProcessor.Data.EntityModels;
using JobProcessor.Data.Enums;
using JobProcessor.Service.BackgroundJobProcessors;
using JobProcessor.Service.Configurations;
using JobProcessor.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace JobProcessor.UnitTests
{
    public class IntegerArraySortingBackgroundJobProcessorTests
    {
        [Fact]
        public void SortArrayFromJob_Should_ReturnAJobWithSortedArrayAsTheJobOutput()
        {
            var _mockLogger = new Mock<ILogger<IntegerArraySortingBackgroundJobProcessor>>();
            var _mockJobManager = new Mock<IJobManager>();

            var _options = Options.Create(new BackgroundJobConfig 
            { 
                IsFakeLongRunningTask = false, 
                JobCheckingFrequencyInSeconds = 5 
            });

            var _mapperConfig = new MapperConfiguration(config => config.AddProfile<JobServiceModelProfile>());
            var _mapper = _mapperConfig.CreateMapper();

            var _processor = new IntegerArraySortingBackgroundJobProcessor(
                _mockLogger.Object,
                _mockJobManager.Object,
                _mapper,
                _options
            );

            var _job = new Job
            {
                JobId = Guid.NewGuid(),
                JobStatus = JobStatus.Processing,
                JobProcessingDurationMiliseconds = 0,
                JobEnqueuedDateTimeUtc = DateTime.UtcNow,
                JobInput = "8,5,3,9,2,76",
                JobOutput = string.Empty
            };

            var _processedJob = _processor.SortArrayFromJob(_job);

            Assert.Equal("2,3,5,8,9,76", _processedJob.JobOutput);
        }
    }
}
