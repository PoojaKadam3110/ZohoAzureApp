using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjectsAPI.Model;
using System.Data.SqlClient;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ProjectsAPI.DataAccessLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace ProjectsAPI
{
    public static class CreateProjects
    {
        [FunctionName("CreateProjects")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Projects API" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateRequest), Description = "The request data.")]
        public static async Task<IActionResult> AddProjects(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "CreateProjects")] HttpRequest req,
            ILogger log, ExecutionContext context)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var _input_data = JsonConvert.DeserializeObject<Projects>(requestBody);
            _input_data.isActive = true;
            _input_data.isDeleted = false;
            _input_data.CreatedDate = DateTime.Now;
            _input_data.CreatedBy = "Pooja";
            _input_data.UpdatedDate = DateTime.Now;
            _input_data.UpdatedBy = "Pooja";
            try
            {

                string defaultConnection = Environment.GetEnvironmentVariable("DBConnections");
                var options = new DbContextOptionsBuilder<SQLDBContext>();
                options.UseSqlServer(defaultConnection);

                var _dbContext = new SQLDBContext(options.Options);

                _dbContext.Projects.Add(_input_data);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return new ObjectResult(e.ToString());
            }
            log.LogInformation("# HTTP Trigger Project Save successfully!!!");
            //return new OkObjectResult(HttpStatusCode.Created);

            return new ObjectResult(_input_data)
            {
                StatusCode = (int)HttpStatusCode.Created
            };
        }
        public class CreateRequest
        {
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


