using IssueManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.DTOs.Issue
{
    public class IssueListDto
    {
        public int Id { get; set; }
        public IssueStatus Status { get; set; }
        public string? Version { get; set; }
    }
}
