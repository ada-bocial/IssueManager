using IssueManager.Domain.IRepositories;
using IssueManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly ManagerDbContext _dbContext;

        public Repository(ManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public T Create(T obj)
        {
            _dbContext.Set<T>().Add(obj);
            return obj;
        }

        public bool Delete(T obj)
        {
            try
            {
                _dbContext.Set<T>().Remove(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        public T? GetById(int? id)
        {
            return _dbContext.Set<T>().Where(x => x.Id == id).FirstOrDefault();
        }


        public T Update(T obj)
        {
            _dbContext.Set<T>().Update(obj);
            return obj;
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

    }
}
