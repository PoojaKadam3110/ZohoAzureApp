using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectsAPI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProjectsAPI.DataAccessLayer
{
    public class SQLDBContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public SQLDBContext(DbContextOptions<SQLDBContext> options) : base(options)
        {
            //_configuration = configuration;
        }
        public virtual DbSet<Projects> Projects
        {
            get;
            set;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string connectionString = _configuration.GetConnectionString("DBConnections");
            SqlConnection connectionString = new SqlConnection(Environment.GetEnvironmentVariable("DBConnections"));

            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            });

            base.OnConfiguring(optionsBuilder);
        }
    }
}
