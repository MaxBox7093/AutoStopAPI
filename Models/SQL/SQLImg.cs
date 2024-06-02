using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AutoStopAPI.Models.SQL
{
    public class SQLImg : SQLConnectionDb
    {
        private readonly SqlConnection _connection;

        public SQLImg()
        {
            _connection = ConnectionDB();
        }

        public bool UpdateUserImage(string phone, byte[] image)
        {
            string query = "UPDATE [dbo].[Users] SET img = @img WHERE phone = @phone";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@phone", phone);
                command.Parameters.AddWithValue("@img", image);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    // Log exception or handle accordingly
                    return false;
                }
            }
        }

        public byte[] GetUserImage(string phone)
        {
            string query = "SELECT img FROM [dbo].[Users] WHERE phone = @phone";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@phone", phone);
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            return (byte[])reader["img"];
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Log exception or handle accordingly
                }
            }
            return null!;
        }
    }
}
