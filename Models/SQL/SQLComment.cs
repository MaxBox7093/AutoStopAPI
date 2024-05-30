using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace AutoStopAPI.Models.SQL
{
    public class SQLComment : SQLConnectionDb
    {
        SqlConnection connection;
        public SQLComment()
        {
            this.connection = ConnectionDB();
        }

        public List<Rating> GetRatings(string phone)
        {
            List<Rating> ratings = new List<Rating>();
            string query = @"
        SELECT 
            c.IdComment,
            c.phoneGet, 
            c.phoneSend, 
            uGet.name AS NameGet, 
            uSend.name AS NameSend, 
            c.date AS dateRating, 
            c.comment, 
            c.rating AS ratingTravel
        FROM 
            [dbo].[Comments] c
        JOIN 
            [dbo].[Users] uGet ON c.phoneGet = uGet.phone
        JOIN 
            [dbo].[Users] uSend ON c.phoneSend = uSend.phone
        WHERE 
            c.phoneGet = @phone OR c.phoneSend = @phone;";

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@phone", phone);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Rating rating = new Rating
                            {
                                idComment = reader["IdComment"] as int?,
                                phoneGet = reader["phoneGet"] as string,
                                phoneSend = reader["phoneSend"] as string,
                                NameGet = reader["NameGet"] as string,
                                NameSend = reader["NameSend"] as string,
                                dateRating = reader["dateRating"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["dateRating"]) : (DateOnly?)null,
                                comment = reader["comment"] as string,
                                ratingTravel = reader["ratingTravel"] != DBNull.Value ? Convert.ToDouble(reader["ratingTravel"]) : (double?)null
                            };
                            ratings.Add(rating);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception or handle it accordingly
                Console.WriteLine(ex.Message);
            }

            return ratings;
        }


        public int AddRating(Rating rating)
        {
            int newId = -1;
            string query = @"
                INSERT INTO [dbo].[Comments] (phoneGet, phoneSend, comment, rating, date)
                OUTPUT INSERTED.IdComment
                VALUES (@phoneGet, @phoneSend, @comment, @ratingTravel, @dateRating);";

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@phoneGet", rating.phoneGet);
                    command.Parameters.AddWithValue("@phoneSend", rating.phoneSend);
                    command.Parameters.AddWithValue("@comment", rating.comment);
                    command.Parameters.AddWithValue("@ratingTravel", rating.ratingTravel);
                    command.Parameters.AddWithValue("@dateRating", DateTime.Now);

                    newId = (int)command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                // Log exception or handle it accordingly
                Console.WriteLine(ex.Message);
            }

            return newId;
        }

        public bool DeleteRating(int idComment)
        {
            bool isDeleted = false;
            string query = "DELETE FROM [dbo].[Comments] WHERE IdComment = @idComment;";

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idComment", idComment);

                    int rowsAffected = command.ExecuteNonQuery();
                    isDeleted = rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Log exception or handle it accordingly
                Console.WriteLine(ex.Message);
            }

            return isDeleted;
        }
    }
}
