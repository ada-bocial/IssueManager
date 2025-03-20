using IssueManager.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.IServices
{
    public interface IAppUserService
    {
        public Task<string?> LoginAsync(LoginDto dto);
    }
}
