using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using ProjectsAPI.Model;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectsAPI.DataAccessLayer;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.Configuration;
using ProjectsAPI.Services;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Data;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace ProjectsAPI
{
    //private SQLDBContext dataconnection;

    public static class GetAllProjects
    {
        private static readonly IMyService service;
        [FunctionName("GetAllProjects")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Projects API" })]
        //[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Projects), Description = "The request data.")]
        
        public static async Task<List<Projects>> GetAllProjectsDetails(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetAllProjects")] HttpRequest req,
            ILogger log, ExecutionContext context)
        {

            var conn = Environment.GetEnvironmentVariable("DBConnections");
            var projects = new List<Projects>();
            try
            {

           //     var config = new ConfigurationBuilder()
           //.SetBasePath(context.FunctionAppDirectory)
           //.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
           //.AddEnvironmentVariables()
           //.Build();

           //     string connectionString = config["DBConnections"];


                log.LogInformation(conn);
                var options = new DbContextOptionsBuilder<SQLDBContext>();
                options.UseSqlServer(conn);

                var _dbContext = new SQLDBContext(options.Options);

                projects = await _dbContext.Projects.Where(p => !p.isDeleted).ToListAsync();
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }

            log.LogInformation("C# HTTP trigger function Display Projects Successfully!!!");
            return projects;

        }
    }
}


//for serices

//var data = await service.GetAllAsync();
//var result = data.Where(x => !x.isDeleted);







//     try
//            {
//                var config = new ConfigurationBuilder()
//                    .SetBasePath(context.FunctionAppDirectory)
//                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
//                    .AddEnvironmentVariables()
//                    .Build();

//    var optionsBuilder = new DbContextOptionsBuilder<SQLDBContext>()
//        .UseSqlServer(config.GetConnectionString("DBConnections"));


//                using (var dbContext = new SQLDBContext(optionsBuilder.Options, config))
//                {
//                    var projects = await dbContext.Projects.ToListAsync();
//                    return new OkObjectResult(projects);
//}
//            }
//            catch (Exception ex)
//            {
//    log.LogError(ex, "An error occurred while fetching projects.");
//    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
//}
//        }







//conn not able to open
//                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionStrings:DBConnections")))
//                {
//                    connection.Open();
//                    var query = @"SELECT * FROM Projects";
//    SqlCommand command = new SqlCommand(query, connection);
//    var reader = await command.ExecuteReaderAsync();
//                    while (reader.Read())
//                    {
//                        Projects projectsdata = new Projects()
//                        {
//                            Id = Convert.ToInt32(reader["Id"]),
//                            ProjectName = Convert.ToString(reader["ProjectName"]),
//                            ClientName = Convert.ToString(reader["ClientName"]),
//                            projectCost = Convert.ToInt32(reader["projectCost"]),
//                            projectManager = Convert.ToString(reader["projectManager"]),
//                            ratePerHour = Convert.ToInt32(reader["ratePerHour"]),
//                            projectUsers = Convert.ToString(reader["projectUsers"]), //
//                            description = Convert.ToString(reader["description"]),
//                            isActive = Convert.ToBoolean(reader["isActive"]),       //
//                            isDeleted = Convert.ToBoolean(reader["isDeleted"]),    //
//                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
//                            CreatedBy = Convert.ToString(reader["CreatedBy"]),
//                            UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]),
//                            UpdatedBy = Convert.ToString(reader["UpdatedBy"])
//                        };
//    projects.Add(projectsdata);
//                    }
//                }
//            }
//            catch (Exception e)
//            {
//    log.LogError(e.ToString());
//}

//return projects;

//        }