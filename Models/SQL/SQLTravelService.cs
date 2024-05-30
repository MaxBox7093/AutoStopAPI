using Microsoft.Data.SqlClient;

namespace AutoStopAPI.Models.SQL
{
    public class SQLTravelService : SQLConnectionDb
    {
        SqlConnection connection;
        public SQLTravelService()
        {
            this.connection = ConnectionDB();
        }
        public List<Travel> GetInactiveTravels(DateTime now)
        {
            List<Travel> travels = new List<Travel>();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = this.connection;
                cmd.CommandText = @"
                    SELECT idTravel, carGRZ, startCity, endCity, dateTime, numberPassenger, comment, isActive
                    FROM [dbo].[Travel]
                    WHERE isActive = 1 AND dateTime < @now";

                cmd.Parameters.AddWithValue("@now", now);

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
                                isActive = reader.GetBoolean(7)
                            };

                            travels.Add(travel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }

            return travels;
        }

        public void UpdateTravel(Travel travel)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = this.connection;
                cmd.CommandText = @"
                    UPDATE [dbo].[Travel]
                    SET isActive = @isActive
                    WHERE idTravel = @idTravel";

                cmd.Parameters.AddWithValue("@isActive", travel.isActive);
                cmd.Parameters.AddWithValue("@idTravel", travel.idTravel);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }
        }
    }
}
