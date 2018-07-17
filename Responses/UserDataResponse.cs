
using AuthService.Models;
using Newtonsoft.Json;

namespace AuthService.Responses
{
    public class UserDataResponse
    {
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

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

        public UserDataResponse(User user)
        {
            this.EmailAddress = user.EmailAddress;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Address1 = user.Address1;
            this.Address2 = user.Address2;
            this.City = user.City;
            this.State = user.State;
            this.PostalCode = user.PostalCode;
        }
    }
}