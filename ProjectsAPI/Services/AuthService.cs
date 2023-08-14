using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectsAPI.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;        }
        public async Task<string> GenerateTokenAsync(string userName)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSecretKey"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, userName)
            }),
                Expires = DateTime.UtcNow.AddDays(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        public bool VerifyPassword(string userName, string storedPasswordHash)
        {
            // Split the stored hash and salt
            var hashBytes = Convert.FromBase64String(storedPasswordHash);
            var salt = new byte[16]; // Assuming 128 bits salt length
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Compute the hash of the entered password using the stored salt
            using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userName));

                // Compare the computed hash with the stored hash
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != hashBytes[i + 16]) // Skip the first 16 bytes (salt)
                        return false;
                }
            }
            return true;
        }
    }
}
 