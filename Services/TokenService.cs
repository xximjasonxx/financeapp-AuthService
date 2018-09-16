
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthService.Models;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace AuthService.Services
{
    public static class TokenService
    {
        public static string CreateWebToken(string userId)
        {
            var payload = new Dictionary<string, string>
            {
                { "userId", userId }
            };

            const string secret = "thisisasecretstring";

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        public static string DecryptToken(string token)
        {
            try
            {
                const string secret = "thisisasecretstring";
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                
                var json = decoder.Decode(token, secret, verify: true);
                return JObject.Parse(json)["userId"].Value<string>();
            }
            catch (TokenExpiredException)
            {
                return string.Empty;
            }
            catch (SignatureVerificationException)
            {
                return string.Empty;
            }
        }
    }
}