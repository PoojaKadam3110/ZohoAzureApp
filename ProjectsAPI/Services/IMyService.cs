using ProjectsAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsAPI.Services
{
    public interface IMyService
    {
        Task<IEnumerable<Projects>> GetAllAsync();
        Task<Projects> GetByIdAsync(int id);
        Task<Projects> AddAsync(Projects entity);
        Task<Projects> UpdateAsync(Projects entity, int id);
        Task<Projects> DeleteAsync(int id);
    }
}
