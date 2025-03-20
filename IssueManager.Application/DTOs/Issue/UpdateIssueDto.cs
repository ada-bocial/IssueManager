using IssueManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.DTOs.Issue
{
    public class UpdateIssueDto
    {
        public int Id { get; set; }
        public IssueStatus Status { get; set; }
        public string? Version { get; set; }
        public string? Target { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
    }
}
