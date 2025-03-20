using IssueManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Domain.IRepositories
{
    public interface IRepository<T> where T : Entity
    {
        T Create(T obj);
        T Update(T obj);
        bool Delete(T obj);
        T GetById(int? id);
        List<T> GetAll();
        Task SaveAsync();
    }
}
