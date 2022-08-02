using AutoMapper;
using JobProcessor.API.ApiModels;
using JobProcessor.API.MapperProfiles;
using JobProcessor.Data.EntityModels;
using JobProcessor.Data.Enums;
using JobProcessor.Data.ServiceModels;

namespace JobProcessor.UnitTests
{
    public class MapperTests
    {
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            var _config = new MapperConfiguration(config => {
                config.AddProfile<JobApiCreateModelProfile>();
                config.AddProfile<JobApiReadModelProfile>();
                config.AddProfile<JobServiceModelProfile>();
            });
            _config.AssertConfigurationIsValid();
        }

        [Fact]
        public void JobApiCreateModelProfile_ShouldMapCorrectly_FromJobApiCreateModelToJob()
        {
            var _config = new MapperConfiguration(config => config.AddProfile<JobApiCreateModelProfile>());
            var _mapper = _config.CreateMapper();

            var _jobApiCreateModel = new JobApiCreateModel()
            {
                NumberListInput = new List<int> { 8, 5, 3, 9, 2, 76 }
            };

            var _job = _mapper.Map<Job>(_jobApiCreateModel);

            Assert.Equal(string.Join(",", _jobApiCreateModel.NumberListInput.Select(number => number.ToString())), _job.JobInput);
            Assert.Equal(string.Empty, _job.JobOutput);
            Assert.Equal(0, _job.JobProcessingDurationMiliseconds);
            Assert.Equal(JobStatus.Queued, _job.JobStatus);
        }

        [Fact]
        public void JobApiReadModelProfile_ShouldMapCorrectly_FromJobToJobApiReadModel()
        {
            var _config = new MapperConfiguration(config => config.AddProfile<JobApiReadModelProfile>());
            var _mapper = _config.CreateMapper();

            var _job = new Job()
            {
                JobInput = "8,5,3,9,2,76",
                JobOutput = "2,3,5,8,9,76",
                JobStatus = JobStatus.Queued
            };

            var _jobApiReadModel = _mapper.Map<JobApiReadModel>(_job);

            Assert.Equal(_job.JobInput.Split(',', System.StringSplitOptions.None).Select(int.Parse), _jobApiReadModel.JobInput);
            Assert.Equal(_job.JobOutput.Split(',', System.StringSplitOptions.None).Select(int.Parse), _jobApiReadModel.JobOutput);
            Assert.Equal(_job.JobStatus.ToString(), _jobApiReadModel.JobStatus);
        }

        [Fact]
        public void JobServiceModelProfile_ShouldMapCorrectly_FromJobToJobServiceModel()
        {
            var _config = new MapperConfiguration(config => config.AddProfile<JobServiceModelProfile>());
            var _mapper = _config.CreateMapper();

            var _job = new Job()
            {
                JobInput = "8,5,3,9,2,76",
                JobOutput = "2,3,5,8,9,76"
            };

            var _jobServiceModel = _mapper.Map<JobServiceModel>(_job);

            Assert.Equal(_job.JobInput.Split(',', System.StringSplitOptions.None).Select(int.Parse), _jobServiceModel.JobInput);
            Assert.Equal(_job.JobOutput.Split(',', System.StringSplitOptions.None).Select(int.Parse), _jobServiceModel.JobOutput);
        }

        [Fact]
        public void JobServiceModelProfile_ShouldMapCorrectly_FromJobServiceModelToJob()
        {
            var _config = new MapperConfiguration(config => config.AddProfile<JobServiceModelProfile>());
            var _mapper = _config.CreateMapper();

            var _jobServiceModel = new JobServiceModel()
            {
                JobInput = new List<int> { 8, 5, 3, 9, 2, 76 },
                JobOutput = new List<int> { 2, 3, 5, 8, 9, 76 }
            };

            var _job = _mapper.Map<Job>(_jobServiceModel);

            Assert.Equal(string.Join(",", _jobServiceModel.JobInput.Select(number => number.ToString())), _job.JobInput);
            Assert.Equal(string.Join(",", _jobServiceModel.JobOutput.Select(number => number.ToString())), _job.JobOutput);
        }
    }
}