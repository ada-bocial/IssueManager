using IssueManager.Application.DTOs.Project;
using IssueManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.IServices
{
    public interface IProjectService
    {
        Task<ProjectDetailsDto> CreateProjectAsync(NewProjectDto dto);
        Task<bool> DeleteProjectAsync(int id, bool isForced);
        List<ProjectsListDto> GetAllProjects();
        Task<ProjectDetailsDto> GetProjectDetailsAsync(int id);
        Task<ProjectDetailsDto> UpdateProjectAsync(UpdateProjectDto dto);
    }
}
