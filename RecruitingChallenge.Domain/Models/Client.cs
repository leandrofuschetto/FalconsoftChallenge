using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingChallenge.Domain.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } 
        public string Email { get; set; }
    }
}
