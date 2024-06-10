using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoStopAPI.Models.SQL
{
    public class SQLChat : SQLConnectionDb
    {
        private SqlConnection connection;
        public SQLChat()
        {
            this.connection = ConnectionDB();
        }

        //Создание чата, но если чат создан то возвращаются и чат и сообщения
        public async Task<Chat> CreateChatAsync(Chat chat)
        {
            try
            {
                string query = @"SELECT idChat, dateCreate, deleteUser1, deleteUser2 FROM Chats 
                         WHERE (phoneUser1 = @phoneUser1 AND phoneUser2 = @phoneUser2) 
                            OR (phoneUser1 = @phoneUser2 AND phoneUser2 = @phoneUser1)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@phoneUser1", chat.phoneUser1);
                    command.Parameters.AddWithValue("@phoneUser2", chat.phoneUser2);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            chat.idChat = reader.GetInt32(0);
                            chat.dateCreate = reader.GetDateTime(1);
                            chat.deleteUser1 = reader.GetBoolean(2);
                            chat.deleteUser2 = reader.GetBoolean(3);

                            SQLMessage sqlMessage = new SQLMessage();
                            chat.messages = await sqlMessage.GetMessageAsync(chat);
                            return chat; // Чат уже существует, возвращаем полный объект чата
                        }
                    }
                }

                string insertQuery = @"
                            INSERT INTO Chats (phoneUser1, phoneUser2, dateCreate, deleteUser1, deleteUser2) 
                            OUTPUT INSERTED.idChat, INSERTED.dateCreate, INSERTED.deleteUser1, INSERTED.deleteUser2
                            VALUES (@phoneUser1, @phoneUser2, @dateCreate, @deleteUser1, @deleteUser2)";

                using (var insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@phoneUser1", chat.phoneUser1);
                    insertCommand.Parameters.AddWithValue("@phoneUser2", chat.phoneUser2);
                    insertCommand.Parameters.AddWithValue("@dateCreate", DateTime.Now);
                    insertCommand.Parameters.AddWithValue("@deleteUser1", false);
                    insertCommand.Parameters.AddWithValue("@deleteUser2", false);

                    using (var reader = await insertCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            chat.idChat = reader.GetInt32(0);
                            chat.dateCreate = reader.GetDateTime(1);
                            chat.deleteUser1 = reader.GetBoolean(2);
                            chat.deleteUser2 = reader.GetBoolean(3);
                        }
                    }

                    return chat;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null!;
        }

        //Получение всех чатов по номеру телефона
        public async Task<List<Chat>> GetChatsAsync(string phone)
        {
            List<Chat> chats = new List<Chat>();
            try
            {
                string query = @"
                    SELECT idChat, dateCreate, deleteUser1, deleteUser2, 
                           CASE 
                               WHEN phoneUser1 = @phoneNumber THEN phoneUser1 
                               ELSE phoneUser2 
                           END AS phoneUser1, 
                           CASE 
                               WHEN phoneUser1 = @phoneNumber THEN phoneUser2 
                               ELSE phoneUser1 
                           END AS phoneUser2
                    FROM Chats
                    WHERE phoneUser1 = @phoneNumber OR phoneUser2 = @phoneNumber";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@phoneNumber", phone);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Chat chat = new Chat
                            {
                                idChat = reader.GetInt32(0),
                                dateCreate = reader.GetDateTime(1),
                                deleteUser1 = reader.GetBoolean(2),
                                deleteUser2 = reader.GetBoolean(3),
                                phoneUser1 = reader.GetString(4),
                                phoneUser2 = reader.GetString(5)
                            };

                            chats.Add(chat);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return chats;
        }
    }
}
