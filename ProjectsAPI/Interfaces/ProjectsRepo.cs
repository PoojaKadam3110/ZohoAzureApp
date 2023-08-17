using ProjectsAPI.DataAccessLayer;
using ProjectsAPI.Generic;
using ProjectsAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsAPI.Interfaces
{
    public class ProjectsRepo : GenericRepository<Projects>, IProjectsRepo
    {
        private readonly IDbConnection _connection;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectsRepo(IDbConnection dbConnection) : base(dbConnection)
        {

        }
    }
}
