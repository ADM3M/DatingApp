using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string pwd { get; set; }
    }
}