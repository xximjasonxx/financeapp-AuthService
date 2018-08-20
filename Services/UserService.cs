
using System.Threading.Tasks;
using AuthService.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AuthService.Services
{
    public static class UserService
    {
        public static async Task<User> FindUserByCredentials(string emailAddress, string password)
        {
            var client = new MongoClient("mongodb://financeapp:alxvP9nMsU21vn6Ap0iLWnPRiKvqauHMDm0SK9jI8OwfNqIfluujL532VHqjZPg61668dt5VWAFbO2DoYpETIg==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("users");
            var collection = database.GetCollection<User>("users");

            var result = await collection.FindAsync(x => x.EmailAddress.ToLower() == emailAddress.ToLower());
            return result.FirstOrDefault();
        }

        public static async Task<User> CreateUser(User newUser)
        {
            var client = new MongoClient("mongodb://financeapp:alxvP9nMsU21vn6Ap0iLWnPRiKvqauHMDm0SK9jI8OwfNqIfluujL532VHqjZPg61668dt5VWAFbO2DoYpETIg==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("users");
            var collection = database.GetCollection<User>("users");
            await collection.InsertOneAsync(newUser);

            return newUser;
        }

        public static async Task<User> GetUserById(string userId)
        {
            var client = new MongoClient("mongodb://financeapp:alxvP9nMsU21vn6Ap0iLWnPRiKvqauHMDm0SK9jI8OwfNqIfluujL532VHqjZPg61668dt5VWAFbO2DoYpETIg==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("users");
            var collection = database.GetCollection<User>("users");
            
            var result = await collection.FindAsync(x => x._Id == ObjectId.Parse(userId));
            return result.FirstOrDefault();
        }
    }
}