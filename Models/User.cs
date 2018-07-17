
using AuthService.Requests;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace AuthService.Models
{
    public class User
    {
        [BsonId]
        public ObjectId _Id { get; set; }

        [BsonIgnore]
        public string Id
        {
            get { return _Id.ToString(); }
            set
            {
                ObjectId parsedValue;
                if (ObjectId.TryParse(value, out parsedValue))
                    _Id = parsedValue;
                else
                    _Id = ObjectId.Empty;
            }
        }

        public string EmailAddress { get; set; }
        public virtual string Password { get; set; }
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