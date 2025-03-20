using IssueManager.Domain.Model;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.Utils
{
    public static class TokenHelper
    {
        public static string GetToken(ApplicationUser userModel)
        {
           
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, userModel.Username),
            new Claim(ClaimTypes.Role, userModel.Role)
        };

            return CreateToken(30, claims);
        }

        private static string CreateToken(int expireDays, Claim[] claims)
        {
            var secret = "660a44324e36fb9baeeb50f0389e8cf501b7a9b4a7517e9736dd7d3aa51db52d586f1cdf889a63ed4c242420284ea962602fdc11e18c5b9589b646dcb6593038e3ce97557939dd7da165dd18acbe65f2aefcefc177e78c78499828f9fa42d88b1cb6fc6ae060c1930d691df8e490e0b2a250c115e48477fa54141628a18ca12c7848f7a14f76647c72da9d9b6a4747471a218d555a147f3f8339f7fbd1e9976e3b38212192d4146867bccc89b1b417910cc45ede542d3a7f29de4c39a28e71e6f12090574bd8a9d5f62b298d15bd73a946c9151e4a4a2cebf89ba98732782335020823bca7d778b1178b9f33730bf16d416020caaa17130d07d45736999c8529ce0250f5c6803aa5422ea283c7cfd1be4ceb6faf9e32a63b0da4f6e4a4d5e2ee60d8a6fe9e85d80c5a64b497e8e49cc7c75057979a524a7a52b19c57cd477ef6c8373f0181aad916b8854def796cc2d1ddcae6d47e101e18e8895315101feff72ce485a5f31dd4b3c8f0f7e7566564834752b1e1e12cf0dd67491676910d38bb58dedd26412665d4a27a0a18caa5c8df6e0c85d83e3f4cec43b91d0829b131d6a834f5965972acfdf87f0e5bca09d0bb7c47b000a671732b939a8ea9c109db1fe65584d63c393f6ba9a243bcb6e8e104e57a51e147bd603fb5651711a5e680e27360ae05cffcfdfcea9230de2d59b0dde049b829a0f7efda2437d7174011e543";

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

            var token = new JwtSecurityToken
            (
                issuer: "defaultIssuer",
                audience: "defaultAudience",
                expires: DateTime.UtcNow.AddMonths(1),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    
    
}
