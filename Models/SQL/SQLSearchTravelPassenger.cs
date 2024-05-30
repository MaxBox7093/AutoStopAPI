using Microsoft.Data.SqlClient;
using AutoStopAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoStopAPI.Models.SQL
{
    public class SQLSearchTravelPassenger : SQLConnectionDb
    {
        private SqlConnection connection;

        public SQLSearchTravelPassenger()
        {
            this.connection = ConnectionDB();
        }

        public List<Travel> SearchTravel(PassengerSearch passenger)
        {
            var result = new List<Travel>();

            try
            {
                string query = @"
        SELECT t.idTravel, t.carGRZ, t.startCity, t.endCity, t.dateTime, t.numberPassenger, 
               t.comment, t.isActive, 
               COALESCE(SUM(p.numberPassenger), 0) AS currentPassengers,
               STRING_AGG(p.phoneTraveler, ',') AS phonePassengers,
               dt.phoneDriver
        FROM Travel t
        LEFT JOIN Passenger p ON t.idTravel = p.idTravel
        LEFT JOIN DriverTravel dt ON t.idTravel = dt.idTravel
        WHERE t.startCity = @From AND t.endCity = @To AND CAST(t.dateTime AS DATE) = @Date
        GROUP BY t.idTravel, t.carGRZ, t.startCity, t.endCity, t.dateTime, t.numberPassenger, 
                 t.comment, t.isActive, dt.phoneDriver
        HAVING COALESCE(SUM(p.numberPassenger), 0) + @Passengers <= t.numberPassenger";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@From", passenger.startCity);
                    command.Parameters.AddWithValue("@To", passenger.endCity);
                    command.Parameters.AddWithValue("@Date", passenger.date);
                    command.Parameters.AddWithValue("@Passengers", passenger.numberPassenger);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var travel = new Travel
                            {
                                idTravel = Convert.ToInt32(reader["idTravel"]),
                                carGRZ = reader["carGRZ"].ToString(),
                                startCity = reader["startCity"].ToString(),
                                endCity = reader["endCity"].ToString(),
                                dateTime = Convert.ToDateTime(reader["dateTime"]),
                                numberPassenger = Convert.ToInt32(reader["numberPassenger"]),
                                comment = reader["comment"].ToString(),
                                isActive = reader["isActive"] as bool?,
                                Passengers = new List<Passenger>(),
                                phoneDriver = reader["phoneDriver"].ToString()
                            };

                            if (reader["phonePassengers"] != DBNull.Value)
                            {
                                var phonePassengers = reader["phonePassengers"].ToString().Split(',');
                                foreach (var phone in phonePassengers)
                                {
                                    travel.Passengers.Add(new Passenger
                                    {
                                        PhonePassenger = phone
                                    });
                                }
                            }

                            result.Add(travel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Console.WriteLine("Ошибка при поиске поездки: " + ex.Message);
            }

            return result;
        }
    }
}
