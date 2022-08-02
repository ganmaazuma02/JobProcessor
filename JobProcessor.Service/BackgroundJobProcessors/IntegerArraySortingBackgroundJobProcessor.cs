using AutoMapper;
using JobProcessor.Data.EntityModels;
using JobProcessor.Data.Enums;
using JobProcessor.Data.ServiceModels;
using JobProcessor.Service.Configurations;
using JobProcessor.Service.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobProcessor.Service.BackgroundJobProcessors
{
    public class IntegerArraySortingBackgroundJobProcessor : BackgroundService, IIntegerArraySortingBackgroundJobProcessor
    {
        private readonly ILogger<IntegerArraySortingBackgroundJobProcessor> _logger;
        private readonly IJobManager _jobManager;
        private readonly IMapper _mapper;
        private readonly BackgroundJobConfig _config;

        public IntegerArraySortingBackgroundJobProcessor(
            ILogger<IntegerArraySortingBackgroundJobProcessor> logger,
            IJobManager jobManager,
            IMapper mapper,
            IOptions<BackgroundJobConfig> config)
        {
            _logger = logger;
            _jobManager = jobManager;
            _mapper = mapper;
            _config = config.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var _delay = TimeSpan.FromSeconds(_config.JobCheckingFrequencyInSeconds);

            _logger.LogInformation(
                $"Background job processor is checking for queued jobs every {_config.JobCheckingFrequencyInSeconds} seconds");

            while (!cancellationToken.IsCancellationRequested)
            {
                await ProcessJob();

                await Task.Delay(_delay, cancellationToken);
            }
        }
        public Job SortArrayFromJob(Job job)
        {
            // just to mimic long running task
            if (_config.IsFakeLongRunningTask)
                Thread.Sleep(7000);

            var _jobServiceModel = _mapper.Map<JobServiceModel>(job);

            _jobServiceModel.JobOutput = _jobServiceModel.JobInput.OrderBy(input => input);

            return _mapper.Map<Job>(_jobServiceModel);
        }

        private async Task ProcessJob()
        {
            var _earliestQueuedJob = await _jobManager.GetEarliestQueuedJobAsync();

            if (_earliestQueuedJob == null)
            {
                _logger.LogInformation(
                    "Background job processor has not found any queued job. Skipping the process.");
                return;
            }

            _logger.LogInformation(
                "Background job processor found a queued job to be processed. Processing the job. JobId: {_earliestQueuedJob.JobId}", _earliestQueuedJob.JobId);

            _earliestQueuedJob.JobStatus = JobStatus.Processing;
            await _jobManager.CommitDatabaseChangesAsync();

            var _stopwatch = new Stopwatch();
            _stopwatch.Start();

            var _updatedJob = SortArrayFromJob(_earliestQueuedJob);

            _stopwatch.Stop();

            _earliestQueuedJob.JobOutput = _updatedJob.JobOutput;
            _earliestQueuedJob.JobStatus = JobStatus.Completed;
            _earliestQueuedJob.JobProcessingDurationMiliseconds = _stopwatch.ElapsedMilliseconds;

            await _jobManager.CommitDatabaseChangesAsync();

            _logger.LogInformation(
                "Background job processor has completed processing the job. JobId: {_earliestQueuedJob.JobId}", _earliestQueuedJob.JobId);

        }
    }
}
