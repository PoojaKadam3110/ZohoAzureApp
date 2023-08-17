using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectsAPI.DataAccessLayer;
using ProjectsAPI.Generic;
using ProjectsAPI.Interfaces;
using ProjectsAPI.Services;
using System;
using System.Data;
using System.Data.SqlClient;

[assembly: FunctionsStartup(typeof(ProjectsAPI.Startup))]
namespace ProjectsAPI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var deploymentLocation = Environment.GetEnvironmentVariable("DeploymentEnvironment");

            Console.WriteLine("Your Project is running on the " + deploymentLocation + " environment");
            ConfigureServices(builder.Services, builder.GetContext().Configuration);
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext
            services.AddDbContext<SQLDBContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("DBConnections")));
            Console.WriteLine("your connection string is " + Environment.GetEnvironmentVariable("DBConnections"));



            string connectionString = "Server=10.235.3.8\\SQL2019STDMPNNEW;DataBase=TSProjectDetailsZoho;User=sa;Password=zcon@123;TrustServerCertificate=true;";

            services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));


            // Other service configurations...
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IMyService, MyService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProjectsRepo,ProjectsRepo>();   
        }
    }
}
