using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectsAPI.DataAccessLayer;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using ProjectsAPI.Model;
using Microsoft.OpenApi.Models;
using static ProjectsAPI.UpdateProjectFunction;

namespace ProjectsAPI
{
    public static class ProjectByID
    {
        [FunctionName("ProjectByID")]
        //[OpenApiOperation(operationId: "Run", tags: new[] { "Projects API" })]
        //[OpenApiParameter(name: "Id", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The **Id** parameter")]
        ////[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ByIdRequest), Description = "The request data.")]

        public static async Task<IActionResult> ProjectByIDFunction(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ProjectByID/{Id}")] HttpRequest req,
            int Id, ILogger log, ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                //var config = new ConfigurationBuilder()
                //    .SetBasePath(context.FunctionAppDirectory)
                //    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                //    .AddEnvironmentVariables()
                //    .Build();

                //string connectionString = config["DBConnections"];

                var conn = Environment.GetEnvironmentVariable("DBConnections");

                var optionsBuilder = new DbContextOptionsBuilder<SQLDBContext>();
                optionsBuilder.UseSqlServer(conn);

                using var dbContext = new SQLDBContext(optionsBuilder.Options);

                var projectToUpdate = await dbContext.Projects.FindAsync(Id);

                if (projectToUpdate == null || projectToUpdate.isDeleted == true)
                {
                    return new NotFoundObjectResult("Project Id " + Id + " Not Found!!!");
                }
                log.LogInformation("C# HTTP trigger function Project of Id " + Id + " display Successfully!!!");

                return new OkObjectResult(projectToUpdate);
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        public class ByIdRequest
        {
            public int Id { get; set; }
        }
    }
}
