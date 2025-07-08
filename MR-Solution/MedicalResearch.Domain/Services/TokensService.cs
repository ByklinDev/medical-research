using MedicalResearch.Domain.Configurations;
using MedicalResearch.Domain.Interfaces.Service;
using MedicalResearch.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.Domain.Services
{
    public class TokensService(IOptions<JwtConfiguration> jwtConfig) : ITokensService
    {
        public  (string token, DateTime expiration) GenerateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = Encoding.UTF8.GetBytes(jwtConfig.Value.SecretKey);

            
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iss, jwtConfig.Value.Issuer),
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.LastName),
            new Claim("initials", user.Initials),
            new Claim("email", user.Email),
            new Claim("oldPassword", user.Password),

        };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtConfig.Value.ExpirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtConfig.Value.Audience,
                Issuer = jwtConfig.Value.Issuer,                
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), tokenDescriptor.Expires.Value);
        }
    }
}
