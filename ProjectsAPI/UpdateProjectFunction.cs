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
using ProjectsAPI.Model;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;

namespace ProjectsAPI
{
    public static class UpdateProjectFunction
    {
       
        [FunctionName("UpdateProjects")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Projects API" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateRequest), Description = "The request data.")]

        public static async Task<IActionResult> UpdateProjectDetails(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "UpdateProjects")] HttpRequest req,
            ILogger log, ExecutionContext context)
        {            
            try
            {
                var conn = Environment.GetEnvironmentVariable("DBConnections");

                var optionsBuilder = new DbContextOptionsBuilder<SQLDBContext>();
                optionsBuilder.UseSqlServer(conn);

                using var dbContext = new SQLDBContext(optionsBuilder.Options);

              

                // Update project properties based on req.Body or other input
                // For example: projectToUpdate.Name = updatedName;
                // Read the request body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var updatedData = Newtonsoft.Json.JsonConvert.DeserializeObject<Projects>(requestBody);

                int idFromRequestBody = updatedData.Id;

                var projectToUpdate = await dbContext.Projects.FindAsync(idFromRequestBody);

                if (projectToUpdate == null || projectToUpdate.isDeleted == true)
                {
                    return new NotFoundObjectResult("Project Id  Not Found!!!");
                }

                // Update project properties
                projectToUpdate.ProjectName = updatedData.ProjectName;
                projectToUpdate.ClientName = updatedData.ClientName;
                projectToUpdate.projectCost = updatedData.projectCost;
                projectToUpdate.projectManager = updatedData.projectManager;
                projectToUpdate.ratePerHour = updatedData.ratePerHour;
                projectToUpdate.projectUsers = updatedData.projectUsers;
                projectToUpdate.UpdatedDate = DateTime.Now;


                await dbContext.SaveChangesAsync();
                log.LogInformation("C# HTTP trigger function Project of Id Updated Successfully!!!");

                return new OkObjectResult(projectToUpdate);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public class UpdateRequest
        {
            public int Id { get; set; }
            public string projectName { get; set; }
            public string clientName { get; set; }
            public string description { get; set; }
            public double projectCost { get; set; }
            public string projectManager { get; set; }
            public double ratePerHour { get; set; }
            public string projectUsers { get; set; }
        }
    }
}
