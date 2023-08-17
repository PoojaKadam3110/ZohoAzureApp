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
using ProjectsAPI.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using ProjectsAPI.Generic;
using System.Net;

namespace ProjectsAPI
{
    public class UpdateProjectFunction
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectFunction(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [FunctionName("UpdateProjects")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Projects API" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateRequest), Description = "The request data.")]

        public async Task<IActionResult> UpdateProjectDetails(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "UpdateProjects")] HttpRequest req,
            ILogger log)
        {            
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var updatedData = Newtonsoft.Json.JsonConvert.DeserializeObject<Projects>(requestBody);

                int idFromRequestBody = updatedData.Id;

                //var projectToUpdate = await dbContext.Projects.FindAsync(idFromRequestBody);

                var projectToUpdate = await _unitOfWork.Projects.GetByIdAsync(idFromRequestBody);

                if (projectToUpdate == null || projectToUpdate.isDeleted == true)
                {
                    var responseMessageNotFound = "Project Id " + idFromRequestBody + " Not Found!!!";

                    var responseNotFoundObject = new { Id = updatedData.Id, Message = responseMessageNotFound }; //, Id = _input_data.Id, Data = _input_data 
                    return new ObjectResult(responseNotFoundObject)
                    {
                        StatusCode = (int)HttpStatusCode.NotFound
                    };
                }

                var data = await _unitOfWork.Projects.UpdateAsync(updatedData);



               // await dbContext.SaveChangesAsync();
                log.LogInformation("C# HTTP trigger function Project of Id Updated Successfully!!!");

                var responseMessage = "Project Updated successfully!!!";

                var responseObject = new {Id = updatedData.Id, Message = responseMessage}; //, Id = _input_data.Id, Data = _input_data 
                return new ObjectResult(responseObject)
                {
                    StatusCode = (int)HttpStatusCode.OK
                };

                //return new OkObjectResult(updatedData);
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
