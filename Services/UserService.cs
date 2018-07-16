
using System.Threading.Tasks;
using AuthService.Models;
using MongoDB.Driver;

namespace AuthService.Services
{
    public static class UserService
    {
        public static async Task<ValidateUserResult> ValidateUser(string emailAddress, string password)
        {
            var client = new MongoClient("mongodb://financeapp:1e5Q5BuE7wRjGYmPSDj3IHK7gbQifFCvMwx7YoviCrUg88YK1YX3go74vYyeYwlzbrsCOxSfzB8iCVopJ7xHSw==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("users");
            var collection = database.GetCollection<User>("users");

            var validationResult = new ValidateUserResult();
            var result = await collection.FindAsync(x => x.EmailAddress.ToLower() == emailAddress.ToLower());
            var user = result.FirstOrDefault();

            validationResult.IsSuccessful = user != null && user.Password == password;
            if (validationResult.IsSuccessful)
            {
                validationResult.UserId = user.Id;
            }

            return validationResult;
        }
    }
}