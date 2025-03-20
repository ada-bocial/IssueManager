using IssueManager.Application.DTOs.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.IServices
{
    public interface IIssueService
    {
        Task<IssueDetailsDto> CreateIssueAsync(NewIssueDto dto);
        Task<bool> DeleteIssueAsync(int id);
        IssueDetailsDto GetIssueDetails(int id);
        Task<List<IssueDetailsDto>> GetProjectIssuesAsync(int projectId);
        Task<IssueDetailsDto> UpdateIssueAsync(UpdateIssueDto dto);
    }
}
