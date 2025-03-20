using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Domain.Model
{
    public class ApplicationUser : Entity
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Role { get; set; }
        
    }

}
