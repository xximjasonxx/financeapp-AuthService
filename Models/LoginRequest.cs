
using Newtonsoft.Json;

namespace AuthService.Models
{
    public class LoginRequest
    {
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}