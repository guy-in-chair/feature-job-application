using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknorixAPI.Domain.DTOs
{
    public class AuthorizeRequestDto
    {
        [Required]
        [MinLength(32), MaxLength(32)]
        public string AppId { get; set; }

        [Required]
        [MinLength(32), MaxLength(32)]
        public string AppSecret { get; set; }
    }


}
