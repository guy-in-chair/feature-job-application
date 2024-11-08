using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknorixAPI.Application.Abstractions;
using TeknorixAPI.Application.IServices;
using TeknorixAPI.Domain.DTOs;

namespace TeknorixAPI.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("/api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class JobsController : Controller
    {
        private readonly IJobService _jobService;
        private readonly ILocationRepository _locationRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public JobsController(IJobService jobService, ILocationRepository locationRepository, IDepartmentRepository departmentRepository)
        {
            _jobService = jobService;
            _locationRepository = locationRepository;
            _departmentRepository = departmentRepository;
        }

        [HttpPost]
        public IActionResult CreateJob(JobDto body)
        {
            var createdJob = _jobService.AddJob(body);

            return CreatedAtAction(nameof(GetJobById), new { id = createdJob.Id }, createdJob);
        }

        [HttpGet("{id}")]
        public IActionResult GetJobById(int id)
        {
            var job = _jobService.GetJobById(id);
            if (job == null)
            {
                return NotFound();
            }

            var resp = new JobMetaDto
            {
                Id = job.Id,
                Code = job.Code,
                ClosingDate = job.ClosingDate,
                Description = job.Description,
                PostedDate = job.PostedDate,
                Title = job.Title,
                Department = _departmentRepository.Find(d => d.Id == job.DepartmentId).FirstOrDefault(),
                Location = _locationRepository.Find(l => l.Id == job.LocationId).FirstOrDefault()
            };
            return Ok(resp);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, JobDto body)
        {
            var value = await _jobService.UpdateJob(id, body);
            return Ok();
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetPaginatedJobs(JobListRequestDto jobListRequest)
        {
            var resp = await _jobService.GetPaginatedJobs(jobListRequest.PageNo, jobListRequest.PageSize, jobListRequest.Q, jobListRequest.LocationId, jobListRequest.DepartmentId);
            return Ok(resp);
        }
    }
}
