using ClassLibraryDataBaseConnection;
using HospitalManagement.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HospitalManagement.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStringMVC");
        }

        public Users Login(string username, string password)
        {
            using SqlConnection connection = ConnectionManager.OpenConnection(_connectionString);

            var query = @"SELECT u.user_id, u.username, u.password, u.full_name,
                         u.role_id, u.is_active, r.role_name
                  FROM Users u
                  INNER JOIN Role r ON u.role_id = r.role_id
                  WHERE u.username = @Username
                  AND u.password = @Password
                  AND u.is_active = 1";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
            command.Parameters.Add("@Password", SqlDbType.VarChar).Value = password;

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Users
                {
                    UserId = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    FullName = reader.GetString(3),
                    RoleId = reader.GetInt32(4),
                    IsActive = reader.GetBoolean(5),
                    Role = new Role
                    {
                        RoleId = reader.GetInt32(4),
                        RoleName = reader.GetString(6)
                    }
                };
            }

            return null;
        }
    }
}
