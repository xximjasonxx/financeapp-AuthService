
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using AuthService.Models;

namespace AuthService.Functions
{
    public static class AuthFunctions
    {
        [FunctionName("perform_login")]
        public static IActionResult PerformLogin([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]LoginRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            log.Info($"{req.EmailAddress}");

            return new OkResult();
        }
    }
}
