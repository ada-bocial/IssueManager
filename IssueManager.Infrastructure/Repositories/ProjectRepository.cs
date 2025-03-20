using IssueManager.Domain.IRepositories;
using IssueManager.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly ManagerDbContext _DbContext;

        public ProjectRepository(ManagerDbContext dbContext) : base(dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<Project> GetProjectWithIssuesAsync(int id)
        {
            return await _DbContext.Projects.Where(x => x.Id == id).Include(x => x.Issues).FirstOrDefaultAsync();
        }
    }
}
