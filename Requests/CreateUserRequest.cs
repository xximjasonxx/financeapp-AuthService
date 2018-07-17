
using AuthService.Models;
using Newtonsoft.Json;

namespace AuthService.Requests
{
    public class CreateUserRequest
    {
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("zipcode")]
        public string PostalCode { get; set; }

        public static explicit operator User(CreateUserRequest request)
        {
            return new User()
            {
                EmailAddress = request.EmailAddress,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                State = request.State,
                PostalCode = request.PostalCode,
            };
        }
    }
}