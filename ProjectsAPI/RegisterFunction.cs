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

namespace ProjectsAPI
{
    public class RegisterFunction
    {
        //private readonly IUserService _userService;

        //public RegisterFunction(IUserService userService)
        //{
        //    _userService = userService;
        //}

        [FunctionName("Register")]
        public async Task<IActionResult> Register(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Register")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a registration request.");

            try
            {
                // Parse request body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var registerRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<RegisterRequestDto>(requestBody);

                // Validate the model
                var validationContext = new ValidationContext(registerRequest, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(registerRequest, validationContext, validationResults);

                if (!isValid)
                {
                    return new BadRequestObjectResult(validationResults);
                }

                //// Store the registration data
                //var registrationResult = await _userService.RegisterUserAsync(registerRequest);

                //if (!registrationResult.Success)
                //{
                //    return new BadRequestObjectResult(registrationResult.Message);
                //}

                return new OkObjectResult(new { Message = "User registered successfully." });
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
