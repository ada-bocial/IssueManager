using IssueManager.Domain.IRepositories;
using IssueManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Infrastructure.Repositories
{
    public class IssueRepository : Repository<Issue>, IIssueRepository
    {
        public IssueRepository(ManagerDbContext dbContext) : base(dbContext)
        {

        }
    }
}
