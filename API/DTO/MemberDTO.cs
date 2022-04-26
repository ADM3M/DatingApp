using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO
{
    public class MemberDTO
    {
        public MemberDTO(int id, string name, int age, string knownAs, DateTime created, DateTime lastActive, string gender, string introduction, string lookingFor, string interests, string city) 
        {
            this.Id = id;
    this.Name = name;
    this.Age = age;
    this.KnownAs = knownAs;
    this.Created = created;
    this.LastActive = lastActive;
    this.Gender = gender;
    this.Introduction = introduction;
    this.LookingFor = lookingFor;
    this.Interests = interests;
    this.City = city;
   
        }
                public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Gender { get; set; }

        public string Introduction { get; set; }
        
        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public ICollection<PhotoDTO> Photo { get; set; }
    }
}