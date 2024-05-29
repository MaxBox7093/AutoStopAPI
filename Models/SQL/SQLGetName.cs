using Microsoft.Data.SqlClient;
using System;
namespace AutoStopAPI.Models.SQL
{
    public class SQLGetName : SQLConnectionDb
    {
        private SqlConnection connection;
        public SQLGetName()
        {
            this.connection = ConnectionDB();
        }

        public string GetName(string phone)
        {
            string query = @"SELECT name FROM USERS WHERE phone = @phone";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@phone", phone);

                try
                {
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return result.ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    // Обработка исключений
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }
    }
}
