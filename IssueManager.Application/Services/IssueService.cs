using IssueManager.Application.DTOs.Issue;
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
    public class IssueService : IIssueService
    {
        private readonly ILogger<ProjectService> _Logger;
        private readonly IProjectRepository _ProjectRepository;
        private readonly IIssueRepository _IssueRepository;

        public IssueService(ILogger<ProjectService> logger, IProjectRepository projectRepo, IIssueRepository issueRepository)
        {
            _Logger = logger;
            _ProjectRepository = projectRepo;
            _IssueRepository = issueRepository;
        }
        public async Task<IssueDetailsDto> CreateIssueAsync(NewIssueDto dto)
        {
            var project = _ProjectRepository.GetById(dto.ProjectId);
            var issue = new Issue
            {
                Status = dto.Status,
                Description = dto.Description,
                InsDate = DateTime.Now,
                ModDate = DateTime.Now,
                Priority = dto.Priority,
                Target = dto.Target,
                Version = dto.Version,
                Project = project,
            };
            _IssueRepository.Create(issue);
            await _IssueRepository.SaveAsync();
            var output = new IssueDetailsDto
            {
                Id = issue.Id,
                Description = issue.Description,
                Priority = issue.Priority,
                Version = issue.Version,
                Target = issue.Target,
                Status = issue.Status,
                InsDate = issue.InsDate,
                ModDate = issue.ModDate,
            };
            _Logger.LogInformation($"Created new issue of id: {issue.Id}");
            return output;
        }

        public async Task<bool> DeleteIssueAsync(int id)
        {
            try
            {
                var issue = _IssueRepository.GetById(id);
                _IssueRepository.Delete(issue);
                await _IssueRepository.SaveAsync();
                _Logger.LogInformation($"Deleted issue of id: {id}");
                return true;
            }
            catch(Exception ex)
            {
                _Logger.LogError($"Error while trying to delete issue of id: {id}");
                return false; 
            }
        }

        public IssueDetailsDto GetIssueDetails(int id)
        {
            var issue = _IssueRepository.GetById(id);
            var output = new IssueDetailsDto
            {
                Id = issue.Id,
                Description = issue.Description,
                Priority = issue.Priority,
                Version = issue.Version,
                Target = issue.Target,
                Status = issue.Status,
                InsDate = issue.InsDate,
                ModDate = issue.ModDate,
            };
            _Logger.LogInformation($"Got details issue of id: {issue.Id}");
            return output;
        }

        public async Task<List<IssueDetailsDto>> GetProjectIssuesAsync(int projectId)
        {
            var project = await _ProjectRepository.GetProjectWithIssuesAsync(projectId);
            var outputList = new List<IssueDetailsDto>();
            if (project is null || project.Issues is null)
                return outputList;
            foreach(var issue in project.Issues)
            {
                outputList.Add(new IssueDetailsDto
                {
                    Id = issue.Id,
                    Description = issue.Description,
                    Priority = issue.Priority,
                    Version = issue.Version,
                    Target = issue.Target,
                    Status = issue.Status,
                    InsDate = issue.InsDate,
                    ModDate = issue.ModDate,
                });
            }
            _Logger.LogInformation($"Got issues for project Id: {projectId}");
            return outputList;
        }

        public async Task<IssueDetailsDto> UpdateIssueAsync(UpdateIssueDto dto)
        {
            var issue = _IssueRepository.GetById(dto.Id);
            issue.Priority = dto.Priority;
            issue.Version = dto.Version;
            issue.Target = dto.Target;
            issue.Status = dto.Status;
            issue.Description = dto.Description;
            issue.ModDate = DateTime.Now;
            _IssueRepository.Update(issue);
            await _IssueRepository.SaveAsync();

            var output = new IssueDetailsDto
            {
                Id = issue.Id,
                Description = issue.Description,
                Priority = issue.Priority,
                Version = issue.Version,
                Target = issue.Target,
                Status = issue.Status,
                InsDate = issue.InsDate,
                ModDate = issue.ModDate,
            };
            _Logger.LogInformation($"Updated issue of id: {dto.Id}");
            return output;
        }
    }
}
