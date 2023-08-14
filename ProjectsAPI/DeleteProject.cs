using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ProjectsAPI.DataAccessLayer;
using ProjectsAPI.Model;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace ProjectsAPI
{
    public static class DeleteProject
    {
        [FunctionName("DeleteProject")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Projects API" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(DeleteRequest), Description = "The request data.")]

        public static async Task<IActionResult> DeleteProjectFunction(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "DeleteProject")] HttpRequest req,
            ILogger log,ExecutionContext context)
        {            
            try
            {
                var conn = Environment.GetEnvironmentVariable("DBConnections");
                var optionsBuilder = new DbContextOptionsBuilder<SQLDBContext>();
                optionsBuilder.UseSqlServer(conn);

                using var dbContext = new SQLDBContext(optionsBuilder.Options);

                

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var updatedData = Newtonsoft.Json.JsonConvert.DeserializeObject<Projects>(requestBody);

                int idFromRequestBody = updatedData.Id;

                var projectToUpdate = await dbContext.Projects.FindAsync(idFromRequestBody);

                if (projectToUpdate == null || projectToUpdate.isDeleted == true)
                {
                    return new NotFoundObjectResult("Project Id " + idFromRequestBody + " Not Found!!!");
                }

                projectToUpdate.isDeleted = true;
                projectToUpdate.isActive = false;

                await dbContext.SaveChangesAsync();

                log.LogInformation("C# HTTP trigger function Project of Id " + idFromRequestBody + " Deleted Successfully!!!");

                return new OkObjectResult("Project Deleted Successfully of Id "+ idFromRequestBody);

            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public class DeleteRequest
        {
            public double Id { get; set; }
        }
    }
}
