using Microsoft.Data.SqlClient;

namespace AutoStopAPI.Models.SQL
{
    public class SQLDriver : SQLConnectionDb
    {
        private SqlConnection connection;
        public SQLDriver()
        {
            this.connection = ConnectionDB();
        }

        public DateOnly? SerchDrive(string phone)
        {
            string query = "SELECT dateGetDoc FROM Users WHERE phone = @Phone";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Phone", phone);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Проверяем, не является ли значение NULL
                            if (!reader.IsDBNull(0))
                            {
                                DateTime dateGetDoc = reader.GetDateTime(0);
                                return DateOnly.FromDateTime(dateGetDoc);
                            }
                        }
                    }
                }

            // Возвращаем null, если запись не найдена или значение NULL
            return null;
        }

        public bool AddDriver(Driver driver)
        {
            string query = "UPDATE Users SET dateGetDoc = @DateGetDoc WHERE phone = @Phone";

            using (var command = new SqlCommand(query, connection))
            {

                command.Parameters.AddWithValue("@DateGetDoc", driver.dateGetDoc);
                command.Parameters.AddWithValue("@Phone", driver.PhoneNumber);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
    }
}
