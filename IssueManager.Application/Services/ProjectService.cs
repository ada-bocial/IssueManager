using IssueManager.Application.DTOs.Issue;
using IssueManager.Application.DTOs.Project;
using IssueManager.Application.IServices;
using IssueManager.Domain.IRepositories;
using IssueManager.Domain.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ILogger<ProjectService> _Logger;
        private readonly IProjectRepository _ProjectRepository;
        private readonly IIssueRepository _IssueRepository;

        public ProjectService(ILogger<ProjectService> logger, IProjectRepository projectRepo, IIssueRepository issueRepository)
        {
            _Logger = logger;
            _ProjectRepository = projectRepo;
            _IssueRepository = issueRepository;
        }
        public async Task<ProjectDetailsDto> CreateProjectAsync(NewProjectDto dto)
        {
            var project = new Project
            {
                Description = dto.Description,
                Name = dto.Name,
            };
            _ProjectRepository.Create(project);
            await _ProjectRepository.SaveAsync();
            var outputDto = new ProjectDetailsDto
            {
                Id = project.Id,
                Description = project.Description,
                Name = project.Name,
            };
            return outputDto;
        }

        public async Task<bool> DeleteProjectAsync(int id, bool isForced)
        {
            if (!isForced)
            {
                try
                {
                    var project = _ProjectRepository.GetById(id);
                    _ProjectRepository.Delete(project);
                    await _ProjectRepository.SaveAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _Logger.LogError(ex, "DeleteProjectAsync encountered an exception");
                }
            }
            else
            {
                var project = await _ProjectRepository.GetProjectWithIssuesAsync(id);
                if (project is null || project.Issues is null)
                    return true;
                foreach(var issue in project.Issues)
                {
                    _IssueRepository.Delete(issue);
                }
                _ProjectRepository.Delete(project);
                await _ProjectRepository.SaveAsync();
                return true;
            }
            return false;
        }

        public List<ProjectsListDto> GetAllProjects()
        {
            var projectsList = _ProjectRepository.GetAll();

            var outputList = new List<ProjectsListDto>();
            foreach (var project in projectsList)
            {
                outputList.Add(new ProjectsListDto
                {
                    Id = project.Id,
                    Name = project.Name,
                }
                );
            }
            return outputList;
        }

        public async Task<ProjectDetailsDto> GetProjectDetailsAsync(int id)
        {
            var project = await _ProjectRepository.GetProjectWithIssuesAsync(id);
            List<IssueListDto> issues = new();
            foreach(var issue in project.Issues)
            {
                issues.Add(new IssueListDto
                {
                    Status = issue.Status,
                    Id = issue.Id,
                    Version = issue.Version,
                });
            }
            var outputDto = new ProjectDetailsDto
            {
                Id = project.Id,
                Description= project.Description,
                Issues = issues,
                Name = project.Name,
            };
            return outputDto;
        }

        public async Task<ProjectDetailsDto> UpdateProjectAsync(UpdateProjectDto dto)
        {
            var project = _ProjectRepository.GetById(dto.Id);
            project.Name = dto.Name;
            project.Description = dto.Description; 
            _ProjectRepository.Update(project);
            await _ProjectRepository.SaveAsync();
            var outputDto = new ProjectDetailsDto
            {
                Id = project.Id,
                Description = project.Description,
                Name = project.Name,
            };
            return outputDto;

        }
    }
}
