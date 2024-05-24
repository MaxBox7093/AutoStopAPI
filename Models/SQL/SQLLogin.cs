using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Numerics;

namespace AutoStopAPI.Models.SQL
{
    public class SQLLogin : SQLConnectionDb
    {
        private SqlConnection connection;
        public SQLLogin()
        {
            this.connection = ConnectionDB();
        }

        public User Login(Login login)
        {
            User user = null;

            string query = "SELECT [phone], [password], [name], [lastName], [dateOfBirth], [img], [rating] " +
                   "FROM [dbo].[Users] WHERE [phone] = @Phone AND [password] = @Password";

            var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Phone", login.Phone);
            command.Parameters.AddWithValue("@Password", login.Password);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User
                    {
                        Phone = reader.GetString(reader.GetOrdinal("phone")),
                        Password = reader.GetString(reader.GetOrdinal("password")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        LastName = reader.GetString(reader.GetOrdinal("lastName")),
                        DateOfBirth = reader.GetDateTime(reader.GetOrdinal("dateOfBirth")),
                        Img = reader.IsDBNull(reader.GetOrdinal("img")) ? null : (byte[])reader["img"],
                        Rating = reader.IsDBNull(reader.GetOrdinal("rating")) ? null : reader.GetFloat(reader.GetOrdinal("rating"))
                    };
                }
            }

            return user;
        }
    }
}
