
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AuthService.Models;
using AuthService.Requests;
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
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { EmailAddress = emailAddress, Password = password });

                return user;
            }
        }

        public static async Task<User> CreateUser(CreateUserRequest newUser)
        {
            const string sql = "insert into Users(Id, EmailAddress, [Password], FirstName, LastName, Address1, Address2, City, [State], PostalCode) values(@Id, @EmailAddress, @Password, @FirstName, @LastName, @Address1, @Address2, @City, @State, @PostalCode)";
            newUser.Id = Guid.NewGuid();
            using (var connection = GetConnection())
            {
                await connection.ExecuteAsync(sql, newUser);
                return await connection.QueryFirstAsync<User>("select * from Users where Id = @Id", new { Id = newUser.Id });
            }
        }

        public static async Task<User> GetUserById(string userId)
        {
            Guid userIdGuid;
            if (!Guid.TryParse(userId, out userIdGuid))
                return null;

            using (var connection = GetConnection())
            {
                const string sql = "select * from Users where Id = @UserId";
                return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserId = userId });
            }
        }
    }
}