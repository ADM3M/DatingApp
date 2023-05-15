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
        public string Name { get; set; }

        [Required] public string KnownAs { get; set; }

        [Required] public string Gender { get; set; }

        [Required] public DateTime DateOfBirth { get; set; }

        [Required] public string Department { get; set; }

        [Required] public string Cabinet { get; set; }

        [Required]
        [MinLength(4)]
        public string pwd { get; set; }
    }
}