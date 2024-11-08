using TeknorixAPI.Domain.Models;

namespace TeknorixAPI.Domain.DTOs
{
    public class PaginatedJobs
    {
        public int Total { get; set; }
        public List<Job> Data { get; set; }
    }
}
