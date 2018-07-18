
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthService.Models;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

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

        public static async Task SaveToken(string token)
        {
            var client = new MongoClient("mongodb://financeapp:1e5Q5BuE7wRjGYmPSDj3IHK7gbQifFCvMwx7YoviCrUg88YK1YX3go74vYyeYwlzbrsCOxSfzB8iCVopJ7xHSw==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("users");
            var collection = database.GetCollection<UserToken>("user_tokens");

            var userToken = new UserToken() { Token = token, ExpiresAt = DateTime.UtcNow.AddDays(30) };
            await collection.InsertOneAsync(userToken);
        }

        public static async Task<bool> TokenIsValid(string token)
        {
            var client = new MongoClient("mongodb://financeapp:1e5Q5BuE7wRjGYmPSDj3IHK7gbQifFCvMwx7YoviCrUg88YK1YX3go74vYyeYwlzbrsCOxSfzB8iCVopJ7xHSw==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("users");
            var collection = database.GetCollection<UserToken>("user_tokens");

            var result = await collection.FindAsync(x => x.Token == token);
            var userToken = result.FirstOrDefault();
            if (userToken.ExpiresAt >= DateTime.UtcNow)
            {
                return true;
            }

            await collection.DeleteOneAsync(x => x.Token == token);
            return false;
        }
    }
}