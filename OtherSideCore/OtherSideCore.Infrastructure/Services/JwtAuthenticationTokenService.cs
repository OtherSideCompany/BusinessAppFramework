using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OtherSideCore.Application.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Services
{
   public class JwtAuthenticationTokenService : IAuthenticationTokenService
   {
      private readonly string _key;
      private readonly string _issuer;

      public JwtAuthenticationTokenService(IConfiguration configuration)
      {
         _key = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
         _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
      }

      public string GenerateAccessToken(int userId)
      {
         var claims = new[]
         {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        };

         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

         var token = new JwtSecurityToken(
             issuer: _issuer,
             audience: _issuer,
             claims: claims,
             expires: DateTime.UtcNow.AddHours(2),
             signingCredentials: creds);

         return new JwtSecurityTokenHandler().WriteToken(token);
      }

      public string GenerateRefreshToken()
      {
         var randomBytes = new byte[64];
         using var rng = RandomNumberGenerator.Create();
         rng.GetBytes(randomBytes);
         return Convert.ToBase64String(randomBytes);
      }
   }
}
