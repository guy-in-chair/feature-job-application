using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknorixAPI.Application.Abstractions;
using TeknorixAPI.Application.IServices;
using TeknorixAPI.Domain.DTOs;
using TeknorixAPI.Domain.Models;

namespace TeknorixAPI.Application.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public Job AddJob(JobDto body)
        {
            var j = new Job
            {
                Title = body.Title,
                Code = GenerateJobCode(),       
                ClosingDate = body.ClosingDate,
                DepartmentId = body.DepartmentId,
                Description = body.Description,
                LocationId = body.LocationId
            };
            return _jobRepository.Add(j);
        }

        public Job? GetJobById(int id)
        {
            return _jobRepository.Find(j => j.Id == id).FirstOrDefault();
        }

        private string GenerateJobCode()
        {
            var allJobs = _jobRepository.GetAll().ToList();
            int maxId = allJobs.Count != 0 ? allJobs.Max(j => j.Id) : 0;

            return $"JOB-{maxId + 1:D4}";
        }
        public async Task<bool> UpdateJob(int id, JobDto request)
        {
            try
            {
                var jobs = await _jobRepository.FindAsync(j => j.Id == id);
                var job = jobs.FirstOrDefault();

                if (job == null)
                {
                    return false;
                }

                job.Title = request.Title;
                job.Description = request.Description;
                job.LocationId = request.LocationId;
                job.DepartmentId = request.DepartmentId;
                job.ClosingDate = request.ClosingDate;

                await _jobRepository.UpdateAsync(job);
            }
            catch (Exception)
            {
                return false;
            }  
            return true;
        }

        public async Task<PaginatedJobs> GetPaginatedJobs(int pageNo, int pageSize, string q = null, int? locationId = null, int? departmentId = null)
        {
            var jobs = _jobRepository.GetAll().ToList();

            // Filter the jobs based on the search criteria
            var filteredJobs = jobs
                .Where(j => (string.IsNullOrEmpty(q) || j.Title.Contains(q, StringComparison.OrdinalIgnoreCase))
                            && (!locationId.HasValue || j.LocationId == locationId)
                            && (!departmentId.HasValue || j.DepartmentId == departmentId))
                .OrderByDescending(j => j.PostedDate)
                .ToList();

            // Get the total count of filtered jobs
            int total = filteredJobs.Count;

            // Apply pagination using Skip and Take
            var paginatedJobs = filteredJobs
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return the result as a PaginatedJobsResult object
            return new PaginatedJobs
            {
                Total = total,
                Data = paginatedJobs
            };
           
        }
    }
}
