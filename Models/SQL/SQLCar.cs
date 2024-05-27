using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace AutoStopAPI.Models.SQL
{
    public class SQLCar : SQLConnectionDb
    {
        private SqlConnection connection;
        public SQLCar()
        {
            this.connection = ConnectionDB();
        }

        public bool AddCar(Car car)
        {
            try 
            {
                string query = "INSERT INTO Car (GRZ, phoneUser, carModel, color) VALUES (@GRZ, @PhoneUser, @CarModel, @Color)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GRZ", car.GRZ);
                    command.Parameters.AddWithValue("@PhoneUser", car.PhoneUser);
                    command.Parameters.AddWithValue("@CarModel", car.CarModel);
                    command.Parameters.AddWithValue("@Color", car.Color);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            } 
            catch (Exception ex) 
            {
                Console.WriteLine("Ошибка при вставке данных в таблицу Car: " + ex.Message);
            }
            return false;
        }

        public List<Car> GetCarsByUser(string phoneUser)
        {
            List<Car> cars = new List<Car>();
            string query = "SELECT GRZ, phoneUser, carModel, color FROM Car WHERE phoneUser = @PhoneUser";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PhoneUser", phoneUser);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Car car = new Car
                        {
                            GRZ = reader["GRZ"].ToString(),
                            PhoneUser = reader["phoneUser"].ToString(),
                            CarModel = reader["carModel"].ToString(),
                            Color = reader["color"].ToString()
                        };
                        cars.Add(car);
                    }
                }
            }

            return cars;
        }

        public bool DeleteCar(string phoneUser, string grz)
        {
            string query = "DELETE FROM Car WHERE phoneUser = @PhoneUser AND GRZ = @GRZ";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PhoneUser", phoneUser);
                command.Parameters.AddWithValue("@GRZ", grz);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public bool UpdateCar(Car car)
        {
            string query = "UPDATE Car SET GRZ = @NewGRZ, carModel = @NewCarModel, color = @NewColor WHERE phoneUser = @PhoneUser AND GRZ = @GRZ";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PhoneUser", car.PhoneUser);
                command.Parameters.AddWithValue("@GRZ", car.OldGRZ);
                command.Parameters.AddWithValue("@NewGRZ", car.GRZ);
                command.Parameters.AddWithValue("@NewCarModel", car.CarModel);
                command.Parameters.AddWithValue("@NewColor", car.Color);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
    }
}
