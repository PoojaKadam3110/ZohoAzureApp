using ProjectsAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsAPI.Services
{
    public interface IUserService
    {
        Task<RegistrationResult> RegisterUserAsync(RegisterRequestDto request);
    }
}
