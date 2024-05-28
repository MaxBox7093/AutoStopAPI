using Microsoft.Data.SqlClient;

namespace AutoStopAPI.Models.SQL
{
    public class SQLPassenger : SQLConnectionDb
    {
        SqlConnection connection;
        public SQLPassenger()
        {
            this.connection = ConnectionDB();
        }

        public AddPassengerResult AddPassenger(Passenger passenger)
        {
            try
            {
                var checkQuery = "SELECT 1 FROM Passenger WHERE phoneTraveler = @phoneTraveler AND idTravel = @idTravel";
                var checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@phoneTraveler", passenger.PhonePassenger);
                checkCommand.Parameters.AddWithValue("@idTravel", passenger.IdTravel);

                var exists = checkCommand.ExecuteScalar() != null;

                if (exists)
                {
                    return AddPassengerResult.AlreadyExists;
                }

                var insertQuery = "INSERT INTO Passenger (phoneTraveler, idTravel, numberPassenger) VALUES (@phoneTraveler, @idTravel, @number)";
                var insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@phoneTraveler", passenger.PhonePassenger);
                insertCommand.Parameters.AddWithValue("@idTravel", passenger.IdTravel);
                insertCommand.Parameters.AddWithValue("@number", passenger.NumberPassenger);

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return AddPassengerResult.Added;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return AddPassengerResult.Error;
        }

        public bool RemovePassenger(Passenger passenger)
        {
            try
            {
                var deleteQuery = "DELETE FROM Passenger WHERE phoneTraveler = @phoneTraveler AND idTravel = @idTravel";
                var deleteCommand = new SqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@phoneTraveler", passenger.PhonePassenger);
                deleteCommand.Parameters.AddWithValue("@idTravel", passenger.IdTravel);

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }
    }
}
