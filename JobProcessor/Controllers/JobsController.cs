using AutoMapper;
using JobProcessor.API.ApiModels;
using JobProcessor.Data.EntityModels;
using JobProcessor.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobProcessor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobManager _jobManager;
        public readonly IMapper _mapper;
        public JobsController(
            IJobManager jobManager,
            IMapper mapper)
        {
            _jobManager = jobManager;
            _mapper = mapper;
        }

        // GET: api/jobs
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var _jobs = await _jobManager.GetJobsAsync();

            var _jobApiReadModels = _jobs.Select(job => _mapper.Map<JobApiReadModel>(job));
            
            return Ok(_jobApiReadModels);
        }

        // GET api/jobs/ff8c3b6b-e274-4e2d-b624-642445d8f816
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var _job = await _jobManager.GetJobAsync(id);

            var _jobApiReadModel = _mapper.Map<JobApiReadModel>(_job);

            return Ok(_jobApiReadModel);
        }

        // POST api/jobs
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JobApiCreateModel createModel)
        {
            var _newJob = _mapper.Map<Job>(createModel);

            await _jobManager.EnqueueJobAsync(_newJob);

            return CreatedAtAction("Get", _newJob.JobId);
        }

    }
}
