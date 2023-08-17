using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectsAPI.DataAccessLayer;
using ProjectsAPI.Interfaces;
using ProjectsAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsAPI.Generic
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection dbConnection1;
        public UnitOfWork(IProjectsRepo projectsRepo, IDbConnection _dbConnection1)
        {
            Projects = projectsRepo;
            dbConnection1 = _dbConnection1;
        }

        public IProjectsRepo Projects { get; private set; }

        public void Dispose()
        {
            dbConnection1.Dispose();
        }
    }
}