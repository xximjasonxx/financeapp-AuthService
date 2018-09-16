
using System;
using Newtonsoft.Json;

namespace AuthService.Models
{
    public class UserToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expires_at")]
        public DateTime ExpiresAt { get; set; }
    }
}