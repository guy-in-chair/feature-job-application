using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknorixAPI.Domain.DTOs;
using TeknorixAPI.Domain.Models;

namespace TeknorixAPI.Application.IServices
{
    public interface IJobService
    {
        Job AddJob(JobDto job);
        Job? GetJobById(int id);
        Task<bool> UpdateJob(int id, JobDto request);
        Task<PaginatedJobs> GetPaginatedJobs(int pageNo, int pageSize, string q = null, int? locationId = null, int? departmentId = null);
    }
}
