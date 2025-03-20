using IssueManager.Application.DTOs.User;
using IssueManager.Application.IServices;
using IssueManager.Application.Utils;
using IssueManager.Domain.IRepositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly ILogger<AppUserService> _Logger;
        private readonly IAppUserRepository _UserRepository;

        public AppUserService(ILogger<AppUserService> logger, IAppUserRepository UserRep)
        {
            _Logger = logger;
            _UserRepository = UserRep;
        }
        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _UserRepository.LoginAsync(dto.Username, dto.Password);
            if (user == null) 
                return null;
            var token =  TokenHelper.GetToken(user);
            _Logger.LogInformation($"Generated Bearer token for user {dto.Username} : {token}");
            return token;
        }
    }
}
