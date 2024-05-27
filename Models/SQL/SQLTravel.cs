using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AutoStopAPI.Models.SQL
{
    public class SQLTravel : SQLConnectionDb
    {
        private SqlConnection connection;

        public SQLTravel()
        {
            this.connection = ConnectionDB();
        }

        public int? CreateTravel(Travel travel)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = this.connection;
                SqlTransaction transaction = null;

                try
                {
                    // Начинаем транзакцию
                    transaction = connection.BeginTransaction();
                    cmd.Transaction = transaction;

                    // Вставка в таблицу Travel
                    cmd.CommandText = @"
                        INSERT INTO [dbo].[Travel] (carGRZ, startCity, endCity, dateTime, numberPassenger, comment, isActive)
                        VALUES (@carGRZ, @startCity, @endCity, @dateTime, @numberPassenger, @comment, @isActive);
                        SELECT SCOPE_IDENTITY();";

                    cmd.Parameters.AddWithValue("@carGRZ", travel.carGRZ ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@startCity", travel.startCity ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@endCity", travel.endCity ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@dateTime", travel.dateTime ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@numberPassenger", travel.numberPassenger ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@comment", travel.comment ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@isActive", travel.isActive ?? (object)DBNull.Value);

                    int newTravelId = Convert.ToInt32(cmd.ExecuteScalar());

                    // Вставка в таблицу DriverTravel
                    cmd.CommandText = @"
                        INSERT INTO [dbo].[DriverTravel] (phoneDriver, idTravel)
                        VALUES (@phoneDriver, @idTravel);";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@phoneDriver", travel.phoneDriver ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@idTravel", newTravelId);

                    cmd.ExecuteNonQuery();

                    // Коммит транзакции
                    transaction.Commit();

                    // Возвращаем id новой поездки
                    return newTravelId;
                }
                catch (Exception ex)
                {
                    // Откат транзакции в случае ошибки
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    // Обработка ошибки (логирование или повторный выброс исключения)
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                    return null;
                }
            }
        }
        public List<Travel> GetTravelsByDriverPhone(string phoneDriver)
        {
            List<Travel> travels = new List<Travel>();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = this.connection;
                cmd.CommandText = @"
                    SELECT t.idTravel, t.carGRZ, t.startCity, t.endCity, t.dateTime, t.numberPassenger, t.comment, t.isActive
                    FROM [dbo].[Travel] t
                    INNER JOIN [dbo].[DriverTravel] dt ON t.idTravel = dt.idTravel
                    WHERE dt.phoneDriver = @phoneDriver";

                cmd.Parameters.AddWithValue("@phoneDriver", phoneDriver);

                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Travel travel = new Travel
                            {
                                idTravel = reader.GetInt32(0),
                                carGRZ = reader.GetString(1),
                                startCity = reader.GetString(2),
                                endCity = reader.GetString(3),
                                dateTime = reader.GetDateTime(4),
                                numberPassenger = reader.GetInt32(5),
                                comment = reader.IsDBNull(6) ? null : reader.GetString(6),
                                isActive = reader.IsDBNull(7) ? (bool?)null : reader.GetBoolean(7)
                            };
                            travels.Add(travel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Обработка ошибки (логирование или повторный выброс исключения)
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }

            return travels;
        }
    }
}
