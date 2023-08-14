using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectsAPI.DataAccessLayer;
using ProjectsAPI.Services;
using System;

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
                options.UseSqlServer(configuration.GetConnectionString("DBConnections")));

            // Other service configurations...
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IMyService, MyService>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}
