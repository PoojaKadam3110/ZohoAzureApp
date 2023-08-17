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
using ProjectsAPI.Services;
using ProjectsAPI.Generic;
using System.Net;
using Microsoft.CodeAnalysis;

namespace ProjectsAPI
{
    public class DeleteProject
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProject(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [FunctionName("DeleteProject")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Projects API" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(DeleteRequest), Description = "The request data.")]

        public async Task<IActionResult> DeleteProjectFunction(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "DeleteProject")] HttpRequest req,
            ILogger log,ExecutionContext context)
        {            
            try
            {

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var updatedData = Newtonsoft.Json.JsonConvert.DeserializeObject<Projects>(requestBody);

                int idFromRequestBody = updatedData.Id;
              
                var data = await _unitOfWork.Projects.DeleteAsync(idFromRequestBody);


                if(data == 1)
                {
                    log.LogInformation("C# HTTP trigger function Project of Id " + idFromRequestBody + " Deleted Successfully!!!");

                    var responseMessage = "Project Deleted successfully!!!";

                    var responseObject = new { Id = updatedData.Id, Message = responseMessage }; //, Id = _input_data.Id, Data = _input_data 
                    return new ObjectResult(responseObject)
                    {
                        StatusCode = (int)HttpStatusCode.OK
                    };
                }
                var responseMessageNotFound = "Project Id " + idFromRequestBody + " Not Found!!!";

                var responseNotFoundObject = new { Id = updatedData.Id, Message = responseMessageNotFound }; //, Id = _input_data.Id, Data = _input_data 
                return new ObjectResult(responseNotFoundObject)
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };


                //return new OkObjectResult("Project Deleted Successfully of Id "+ idFromRequestBody);

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
