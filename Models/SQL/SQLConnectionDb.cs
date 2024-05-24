using Microsoft.Data.SqlClient;
using System;

namespace AutoStopAPI.Models.SQL
{
    public class SQLConnectionDb
    {
        private string? connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SQLAutoStopAPI;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        protected SqlConnection connection;

        public SqlConnection ConnectionDB()
        {
            try
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    Console.WriteLine("Подключение успешно установлено.");
                }
                else
                {
                    Console.WriteLine("Строка подключения не загружена.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Ошибка подключения к базе данных: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
            }

            return connection;
        }
    }
}
