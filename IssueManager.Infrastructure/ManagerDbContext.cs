using IssueManager.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Infrastructure
{
    public class ManagerDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<ApplicationUser> AppUsers { get; set; }
        public ManagerDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
