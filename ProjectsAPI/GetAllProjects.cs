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
using ProjectsAPI.Generic;
using ProjectsAPI.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.SqlServer.Management.Smo;
using System.ComponentModel;
using System.Net;

namespace ProjectsAPI
{
    //private SQLDBContext dataconnection;

    public class GetAllProjects
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProjects(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        [FunctionName("GetAllProjects")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Projects API" })]
        [OpenApiParameter(name: "projectName", In = ParameterLocation.Query, Required = false, Type = typeof(string))]
        [OpenApiParameter(name: "orderByColumn", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "Id")]
        [OpenApiParameter(name: "isDescending", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "false")]
        [OpenApiParameter(name: "pageSize", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "1000")]
        [OpenApiParameter(name: "pageNumber", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "1")]
        //[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Projects), Description = "The request data.")]

        public async Task<IEnumerable<Projects>> GetAllProjectsDetails(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetAllProjects")] HttpRequest req,
            ILogger log)
        {
            string projectName = null;//req.Query["projectName"];
            if (req.Query.ContainsKey("projectName"))
            {
                projectName = req.Query["projectName"]; 
            }
            string orderByColumn = "Id"; //req.Query["orderByColumn"];
            if (req.Query.ContainsKey("orderByColumn"))
            {
                orderByColumn = req.Query["orderByColumn"];
            }
            bool isDescending = false; //= bool.Parse(req.Query["isDescending"]);
            if (req.Query.ContainsKey("isDescending"))
            {
                isDescending = bool.Parse(req.Query["isDescending"]);
            }
            int pageSize = 1000;//int.Parse(req.Query["pageSize"]);
            if (req.Query.ContainsKey("pageSize"))
            {
                pageSize = int.Parse(req.Query["pageSize"]);
            }
            int pageNumber = 1;// int.Parse(req.Query["pageNumber"]);
            if (req.Query.ContainsKey("pageNumber"))
            {
                pageNumber = int.Parse(req.Query["pageNumber"]);
            }

            var data = await _unitOfWork.Projects.GetAllAsync(projectName, orderByColumn, isDescending, pageSize, pageNumber);

            var result = data.Where(x => !x.isDeleted);

            if (result == null || !result.Any())
            {
                return null;
            }

            return result;
        }

        [FunctionName("GetActiveProjects")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Projects API" })]
        public async Task<IActionResult> GetActiveProjectsDetails(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetActiveProjects")] HttpRequest req,
           ILogger log)
        {
            int count = _unitOfWork.Projects.GetRecordCount();

            if(count == 0)
            {
                var responseMessage = "Not Active Projects in the database";
                var responseNoObject = new { Message = responseMessage };  
                return new ObjectResult(responseNoObject)
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }

            var responseObject = new { Active_Projects = count }; //, Id = _input_data.Id, Data = _input_data 
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.OK
            };
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