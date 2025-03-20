using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Domain.Model
{
    public class Project : Entity
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<Issue>? Issues { get; set; }
    }
}
