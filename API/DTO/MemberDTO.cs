using System;
using System.Collections.Generic;

namespace API.DTO
{
    public class MemberDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string PhotoUrl { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Gender { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string Cabinet { get; set; }

        public string Department { get; set; }

        public ICollection<PhotoDTO> Photos { get; set; }
    }
}