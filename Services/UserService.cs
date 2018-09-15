
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AuthService.Models;
using Dapper;

namespace AuthService.Services
{
    public static class UserService
    {
        static IDbConnection GetConnection()
        {
            var connStr = Environment.GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.Process);
            var connection = new SqlConnection(connStr);
            connection.Open();

            return connection;
        }

        public static async Task<User> FindUserByCredentials(string emailAddress, string password)
        {
            using (var connection = GetConnection())
            {
                var sql = "select * from Users where EmailAddress = @EmailAddress and Password = @Password";
                var user = await connection.QueryFirstAsync<User>(sql, new { EmailAddress = emailAddress, Password = password });

                return user;
            }
        }

        public static async Task<User> CreateUser(User newUser)
        {
            const string sql = "insert into Users(Id, EmailAddress, [Password], FirstName, LastName, Address1, Address2, City, [State], PostalCode) values(@Id, @EmailAddress, @Password, @FirstName, @LastName, @Address1, @Address2, @City, @State, @PostalCode)";
            newUser.Id = Guid.NewGuid().ToString();
            using (var connection = GetConnection())
            {
                await connection.ExecuteAsync(sql, newUser);
            }

            return newUser;
        }

        public static async Task<User> GetUserById(string userId)
        {
            /*var client = new MongoClient("mongodb://financeapp:alxvP9nMsU21vn6Ap0iLWnPRiKvqauHMDm0SK9jI8OwfNqIfluujL532VHqjZPg61668dt5VWAFbO2DoYpETIg==@financeapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
            var database = client.GetDatabase("users");
            var collection = database.GetCollection<User>("users");
            
            var result = await collection.FindAsync(x => x._Id == ObjectId.Parse(userId));
            return result.FirstOrDefault();*/

            return new User(null);
        }
    }
}