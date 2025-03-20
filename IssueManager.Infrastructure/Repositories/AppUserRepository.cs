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
    public class AppUserRepository : Repository<ApplicationUser>, IAppUserRepository
    {
        private readonly ManagerDbContext _DbContext;

        public AppUserRepository(ManagerDbContext dbContext) : base(dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<ApplicationUser?> LoginAsync(string username, string password)
        {
            return await _DbContext.AppUsers.Where(x => x.Username == username && x.Password == password).FirstOrDefaultAsync();
        }
    }
}
