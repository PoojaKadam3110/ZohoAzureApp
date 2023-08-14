using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsAPI.Services
{
    public interface IAuthService
    {
        Task<string> GenerateTokenAsync(string userName);
        bool VerifyPassword(string userName, string storedPasswordHash);
    }
}
