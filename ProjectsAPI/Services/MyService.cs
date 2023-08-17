using Microsoft.EntityFrameworkCore;
using ProjectsAPI.DataAccessLayer;
using ProjectsAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsAPI.Services
{
    public class MyService : IMyService
    {
        private readonly SQLDBContext _dbContext;
        private IDbConnection dbConnection1;

        public MyService(SQLDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Projects> AddAsync(Projects entity)
        {
            var addedEntity = await _dbContext.Projects.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task<Projects> DeleteAsync(int id)
        {
            var existingProject = await _dbContext.Projects.FindAsync(id);

            if (existingProject == null)
            {
                throw new InvalidOperationException("Project not found");
            }

            existingProject.isDeleted = true;
            existingProject.isActive = false;

            await _dbContext.SaveChangesAsync();

            return existingProject;
        }

        public async Task<IEnumerable<Projects>> GetAllAsync()
        {
            return await _dbContext.Projects.Where(p => !p.isDeleted).ToListAsync();
        }

        public async Task<Projects> GetByIdAsync(int id)
        {
            return await _dbContext.Projects.Where(p => !p.isDeleted).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Projects> UpdateAsync(Projects entity, int id)
        {
            var existingProject = await _dbContext.Projects.FindAsync(id);

            if (existingProject == null)
            {
                throw new InvalidOperationException("Project not found");
            }

            existingProject.ProjectName = entity.ProjectName;
            existingProject.ClientName = entity.ClientName;
            existingProject.description = entity.description;
            existingProject.projectCost = entity.projectCost;
            existingProject.projectManager = entity.projectManager;
            existingProject.ratePerHour = entity.ratePerHour;
            existingProject.projectUsers = entity.projectUsers;
            existingProject.UpdatedDate = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return existingProject;
        }
    }

}
