
using System.IO;
using AuthService.Extensions;
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
using Microsoft.Extensions.Logging;

namespace AuthService.Functions
{
    public static class AuthFunctions
    {
        [FunctionName("perform_login")]
        public static async Task<IActionResult> PerformLogin([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]LoginRequest req, TraceWriter log)
        {
            if (string.IsNullOrEmpty(req.EmailAddress) || string.IsNullOrEmpty(req.Password))
                return new BadRequestResult();

            var user = await UserService.FindUserByCredentials(req.EmailAddress, req.Password);
            if (user == null)
                return new NotFoundResult();

            var result = new UserResponse();

            // create the jwt token
            var token = TokenService.CreateWebToken(result.UserId);

            // save the token for validation later
            await TokenService.SaveToken(token);

            return new OkObjectResult(new { token = token });
        }

        [FunctionName("create_user")]
        public static async Task<IActionResult> CreateUser([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]CreateUserRequest request, TraceWriter log)
        {
            var newUser = await UserService.CreateUser(request);
            var token = TokenService.CreateWebToken(newUser.Id.ToString());
            await TokenService.SaveToken(token);

            return new OkObjectResult(new UserResponse
            {
                UserId = newUser.Id.ToString(),
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

            return new OkObjectResult(user);
        }

        [FunctionName("get_user_for_token")]
        public static async Task<IActionResult> GetUserForToken([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req, TraceWriter log)
        {
            var token = req.Headers["auth-key"].ToString().AsJwtToken();
            if (string.IsNullOrEmpty(token))
                return new BadRequestResult();

            var user = await CacheService.GetValueForKey<User>(token);
            if (user == null)
                return new NotFoundResult();

            var userId = TokenService.DecryptToken(token);
            if (userId != user.Id.ToString())
                return new UnauthorizedResult();

            return new OkObjectResult(user);
        }
    }
}
