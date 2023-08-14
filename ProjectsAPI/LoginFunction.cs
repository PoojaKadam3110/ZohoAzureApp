using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjectsAPI.Model.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjectsAPI.Services;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using static ProjectsAPI.UpdateProjectFunction;
using ProjectsAPI.DataAccessLayer;
using Microsoft.Extensions.FileProviders;

namespace ProjectsAPI
{
    public  class LoginFunction
    {
        private  readonly IAuthService _authService;
        public LoginFunction(IAuthService authService)
        {
            _authService = authService;
        }
        [FunctionName("Login")]
        //[OpenApiOperation(operationId: "Run", tags: new[] { "Projects API" })]
        //[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(LoginRequestDto), Description = "The request data.")]

        public  async Task<IActionResult> Login(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Login")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            //return new OkResult();

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var loginRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginRequestDto>(requestBody);


                //if((loginRequest.UserName == "pooja@kadam.com") && (loginRequest.Password == "Admin@123"))
                var name = Environment.GetEnvironmentVariable("DemoUsername");
                var pass = Environment.GetEnvironmentVariable("DemoPassword");
                if ((loginRequest.UserName == name) && (loginRequest.Password == pass))
                {
                    var validationContext = new ValidationContext(loginRequest, serviceProvider: null, items: null);
                    var validationResults = new List<ValidationResult>();
                    bool isValid = Validator.TryValidateObject(loginRequest, validationContext, validationResults);

                    if (!isValid)
                    {
                        return new BadRequestObjectResult(validationResults);
                    }

                    var token = await _authService.GenerateTokenAsync(loginRequest.UserName);

                    if (token == null)
                    {
                        return new UnauthorizedResult();
                    }

                    return new OkObjectResult(new { AccessToken = token });
                }
                else
                {
                    log.LogError("datataaa");
                    return new NotFoundObjectResult("Please Entered valid name or password");
                }

                // Validate the model
               
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
