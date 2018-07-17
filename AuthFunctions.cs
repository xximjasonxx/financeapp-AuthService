
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

namespace AuthService.Functions
{
    public static class AuthFunctions
    {
        [FunctionName("perform_login")]
        public static async Task<IActionResult> PerformLogin([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]LoginRequest req, TraceWriter log)
        {
            if (string.IsNullOrEmpty(req.EmailAddress) || string.IsNullOrEmpty(req.Password))
            {
                return new BadRequestResult();
            }

            var result = await UserService.ValidateUser(req.EmailAddress, req.Password);
            if (!result.IsSuccessful)
            {
                return new ForbidResult();
            }

            // create the jwt token
            var token = TokenService.CreateWebToken(result.UserId);

            // save the token for validation later
            await TokenService.SaveToken(token);

            return new OkObjectResult(token);
        }

        [FunctionName("create_user")]
        public static async Task<IActionResult> CreateUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]User request, TraceWriter log)
        {
            var newUser = await UserService.CreateUser(request);
            var token = TokenService.CreateWebToken(newUser.Id);
            await TokenService.SaveToken(token);

            return new OkObjectResult(newUser);
        }
    }
}
