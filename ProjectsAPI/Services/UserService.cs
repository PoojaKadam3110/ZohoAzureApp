using ProjectsAPI.DataAccessLayer;
using ProjectsAPI.Model;
using ProjectsAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsAPI.Services
{
    public class UserService : IUserService
    {
        private readonly SQLDBContext _dbContext;

        public UserService(SQLDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RegistrationResult> RegisterUserAsync(RegisterRequestDto request)
        {
            // Check if the user already exists
            //if (await _dbContext.Users.AnyAsync(u => u.UserName == request.UserName))
            //{
            //    return new RegistrationResult { Success = false, Message = "User already exists." };
            //}

            // Hash the password and store the user data
            var (hash, salt) = HashPassword(request.Password);
            var user = new User
            {
                UserName = request.UserName,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            //_dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return new RegistrationResult { Success = true, Message = "User registered successfully." };
        }

        private (string Hash, string Salt) HashPassword(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var salt = Convert.ToBase64String(hmac.Key);
                var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password + salt)));
                return (hash, salt);
            }
        }
    }

    public class RegistrationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}

