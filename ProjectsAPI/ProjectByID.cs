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
using ProjectsAPI.Services;
using ProjectsAPI.Generic;
using System.Net;

namespace ProjectsAPI
{
    public class ProjectByID
    {
        private readonly IMyService _service;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectByID(IMyService service,IUnitOfWork unitOfWork)
        {
            _service = service;
            _unitOfWork = unitOfWork;
        }

        [FunctionName("ProjectByID")]
        //[OpenApiOperation(operationId: "Run", tags: new[] { "Projects API" })]
        //[OpenApiParameter(name: "Id", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The **Id** parameter")]
        ////[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ByIdRequest), Description = "The request data.")]

        public async Task<IActionResult> ProjectByIDFunction(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ProjectByID/{Id}")] HttpRequest req,
            int Id, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //var data = await _service.GetByIdAsync(Id);
            var data = await _unitOfWork.Projects.GetByIdAsync(Id);

            //var result = data;
            if (data == null || data.isDeleted == true)
            {
                var responseMessageNotFound = "Project Id " + Id + " Not Found!!!";

                var responseNotFoundObject = new { Id = Id, Message = responseMessageNotFound }; //, Id = _input_data.Id, Data = _input_data 
                return new ObjectResult(responseNotFoundObject)
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            log.LogInformation("C# HTTP trigger function Project of Id " + Id + " display Successfully!!!");
            return new OkObjectResult(data);

            //try
            //{

            //    var conn = Environment.GetEnvironmentVariable("DBConnections");

            //    var optionsBuilder = new DbContextOptionsBuilder<SQLDBContext>();
            //    optionsBuilder.UseSqlServer(conn);

            //    using var dbContext = new SQLDBContext(optionsBuilder.Options);

            //    var projectToUpdate = await dbContext.Projects.FindAsync(Id);

            //    if (projectToUpdate == null || projectToUpdate.isDeleted == true)
            //    {
            //        return new NotFoundObjectResult("Project Id " + Id + " Not Found!!!");
            //    }
            //    log.LogInformation("C# HTTP trigger function Project of Id " + Id + " display Successfully!!!");

            //    return new OkObjectResult(projectToUpdate);
            //}
            //catch (Exception ex)
            //{
            //    log.LogError(ex.ToString());
            //    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            //}
        }
        public class ByIdRequest
        {
            public int Id { get; set; }
        }
    }
}
