
using AuthService.Models;
using Newtonsoft.Json;

namespace AuthService.Responses
{
    public class UserDataResponse : User
    {
        [JsonIgnore]
        public override string Password { get; set; }

        public UserDataResponse(User user)
        {
            this.EmailAddress = user.EmailAddress;
            this.Password = user.Password;
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