
using Newtonsoft.Json;

namespace AuthService.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
}