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
using ProjectsAPI.Services;
using ProjectsAPI.Generic;

namespace ProjectsAPI
{
    public class CreateProjects
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateProjects(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [FunctionName("CreateProjects")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Projects API" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateRequest), Description = "The request data.")]
        public async Task<IActionResult> AddProjects(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "CreateProjects")] HttpRequest req,
            ILogger log, ExecutionContext context)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var _input_data = JsonConvert.DeserializeObject<Projects>(requestBody);
            try
            {
                
                //var data = await _service.AddAsync(_input_data);
                var data = await _unitOfWork.Projects.AddAsync(_input_data);

                var result = data;

                //return new OkObjectResult(result);
                log.LogInformation("# HTTP Trigger Project Save successfully!!!");

                //return new ObjectResult(_input_data)
                //{
                //    StatusCode = (int)HttpStatusCode.Created
                //};

                var responseMessage = "Project Save successfully!!!";
                var responseObject = new { Message = responseMessage}; //, Id = _input_data.Id, Data = _input_data 
                return new ObjectResult(responseObject)
                {
                    StatusCode = (int)HttpStatusCode.Created
                };

            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return new ObjectResult(e.ToString());
            }
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


