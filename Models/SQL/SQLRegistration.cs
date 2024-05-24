using Microsoft.Data.SqlClient;

namespace AutoStopAPI.Models.SQL
{
    public class SQLRegistration: SQLConnectionDb
    {
        private SqlConnection connection;
        public SQLRegistration() 
        {
            this.connection = ConnectionDB();
        }

        public void RegistrationUser(Registration registration) 
        {
            var query = "INSERT INTO Users (phone, password, name, lastName, dateOfBirth) VALUES (@Phone, @Password, @Name, @LastName, @DateOfBirth)";
            
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@phone", registration.Phone);
            command.Parameters.AddWithValue("@password", registration.Password);
            command.Parameters.AddWithValue("@name", registration.Name);
            command.Parameters.AddWithValue("@lastName", registration.LastName);
            command.Parameters.AddWithValue("@dateOfBirth", registration.DateOfBirth);

            command.ExecuteNonQuery();
            
        }
    }
}
