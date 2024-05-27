using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AutoStopAPI.Models.SQL
{
    public class SQLUser : SQLConnectionDb
    {
        SqlConnection connection;
        public SQLUser()
        {
            this.connection = ConnectionDB();
        }

        public bool UpdateUser(User user)
        {
            try
            {
                var query = @"UPDATE Users SET password = @Password, name = @Name, lastName = @LastName, dateOfBirth = @DateOfBirth WHERE phone = @Phone";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Phone", user.Phone);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);

                    command.ExecuteNonQuery();

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
            }

            return false;
        }
    }
}
