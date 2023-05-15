using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO
{
    public class MemberUpdateDTO
    {
        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string Cabinet { get; set; }

        public string Department { get; set; }
    }
}