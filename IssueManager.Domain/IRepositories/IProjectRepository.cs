using IssueManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Domain.IRepositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<Project> GetProjectWithIssuesAsync(int id);
    }
}
