using IssueManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Domain.IRepositories
{
    public interface IAppUserRepository : IRepository<ApplicationUser>
    {
        public Task<ApplicationUser?> LoginAsync(string username, string password);
    }
}
