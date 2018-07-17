
using Newtonsoft.Json;

namespace AuthService.Requests
{
    public class LoginRequest
    {
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}