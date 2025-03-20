using IssueManager.Application.DTOs.Issue;
using IssueManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.DTOs.Project
{
    public class ProjectDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<IssueListDto> Issues { get; set; }
    }
}
