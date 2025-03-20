using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Domain.Model
{
    public class Issue : Entity
    {
        [Required]
        public IssueStatus Status { get; set; }
        public string? Version { get; set; }
        public string? Target{ get; set; }
        [Required]
        public string Description { get; set; }
        public int Priority { get; set; } = 0;
        [Required]
        public DateTime ModDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime InsDate { get; set; } = DateTime.Now;
        public Project Project { get; set; }
    }
    public enum IssueStatus
    {
        Unknown = 0,
        Unconfirmed = 1,
        Confirmed = 2,
        InProgress = 3,
        Resolved = 4,
        Closed = 5
    }
}
