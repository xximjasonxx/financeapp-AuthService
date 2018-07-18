
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using AuthService.Models;
using AuthService.Services;
using System.Threading.Tasks;
using AuthService.Requests;
using AuthService.Responses;
using System;

namespace AuthService.Functions
{
    public static class AuthFunctions
    {
        [FunctionName("perform_login")]
        public static async Task<IActionResult> PerformLogin([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]LoginRequest req, TraceWriter log)
        {
            if (string.IsNullOrEmpty(req.EmailAddress) || string.IsNullOrEmpty(req.Password))
            {
                return new BadRequestResult();
            }

            var user = await UserService.FindUserByCredentials(req.EmailAddress, req.Password);
            if (user == null)
            {
                return new ForbidResult();
            }

            var result = new UserResponse();

            // create the jwt token
            var token = TokenService.CreateWebToken(result.UserId);

            // save the token for validation later
            await TokenService.SaveToken(token);

            return new OkObjectResult(token);
        }

        [FunctionName("create_user")]
        public static async Task<IActionResult> CreateUser([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]CreateUserRequest request, TraceWriter log)
        {
            // todo: add logic to check for duplicates
            var newUser = new User(request);

            newUser = await UserService.CreateUser(newUser);
            var token = TokenService.CreateWebToken(newUser.Id);
            await TokenService.SaveToken(token);

            return new OkObjectResult(new UserResponse
            {
                UserId = newUser.Id,
                Token = token
            });
        }

        [FunctionName("get_user_id")]
        public static async Task<IActionResult> GetUserById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "{id}")]HttpRequest request, string id, TraceWriter log)
        {
            var user = await UserService.GetUserById(id);
            if (user == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(new UserDataResponse(user));
        }

        [FunctionName("validate_token")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
