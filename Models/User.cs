
using AuthService.Requests;
using Newtonsoft.Json;

namespace AuthService.Models
{
    public class User
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

        public User(CreateUserRequest request)
        {
            this.EmailAddress = request.EmailAddress;
            this.Password = request.Password;
            this.FirstName = request.FirstName;
            this.LastName = request.LastName;
            this.Address1 = request.Address1;
            this.Address2 = request.Address2;
            this.City = request.City;
            this.State = request.State;
            this.PostalCode = request.PostalCode;
        }
    }
}