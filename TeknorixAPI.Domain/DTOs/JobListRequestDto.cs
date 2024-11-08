using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknorixAPI.Domain.DTOs
{
    public class JobListRequestDto
    {
        public string Q { get; set; } 
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int? LocationId { get; set; }
        public int? DepartmentId { get; set; } 
    }
}
