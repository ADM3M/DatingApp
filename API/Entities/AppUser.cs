using System;
using System.Collections.Generic;
using API.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime LastActive { get; set; } = DateTime.Now;

        public string Gender { get; set; }

        public string Introduction { get; set; }
        
        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public ICollection<Photos> Photo { get; set; }

        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }
}