using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using AutoStopAPI.Models;

namespace AutoStopAPI.Models.SQL
{
    public class SQLMessage : SQLConnectionDb
    {
        SqlConnection connection;
        public SQLMessage()
        {
            this.connection = ConnectionDB();
        }

        //Используется только в том случае, если создается чат
        public List<Message> GetMessage(Chat chat)
        {
            List<Message> messages = new List<Message>();

            string query = @"
                SELECT idMessage, refChat, senderPhone, content, sendDate 
                FROM message 
                WHERE refChat = (SELECT idChat FROM Chats 
                                 WHERE (phoneUser1 = @phoneUser1 AND phoneUser2 = @phoneUser2) 
                                    OR (phoneUser1 = @phoneUser2 AND phoneUser2 = @phoneUser1))
                ORDER BY sendDate DESC";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@phoneUser1", chat.phoneUser1);
                command.Parameters.AddWithValue("@phoneUser2", chat.phoneUser2);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        messages.Add(new Message
                        {
                            idMessage = reader.GetInt32(0),
                            refChat = reader.GetInt32(1),
                            senderPhone = reader.GetString(2),
                            content = reader.GetString(3),
                            sendDate = reader.GetDateTime(4)
                        });
                    }
                }
            }

            return messages;
        }

        public List<Message> GetMessageById(int idChat)
        {
            List<Message> messages = new List<Message>();

            string query = @"
        SELECT idMessage, refChat, senderPhone, content, sendDate 
        FROM message 
        WHERE refChat = @idChat
        ORDER BY sendDate DESC";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idChat", idChat);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        messages.Add(new Message
                        {
                            idMessage = reader.GetInt32(0),
                            refChat = reader.GetInt32(1),
                            senderPhone = reader.GetString(2),
                            content = reader.GetString(3),
                            sendDate = reader.GetDateTime(4)
                        });
                    }
                }
            }

            return messages;
        }


        public int AddMessage(Message message)
        {
            string query = @"
        INSERT INTO message (refChat, senderPhone, content, sendDate) 
        OUTPUT INSERTED.idMessage
        VALUES (@refChat, @senderPhone, @content, @sendDate)";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@refChat", message.refChat);
                command.Parameters.AddWithValue("@senderPhone", message.senderPhone);
                command.Parameters.AddWithValue("@content", message.content);
                command.Parameters.AddWithValue("@sendDate", DateTime.Now);

                int insertedId = (int)command.ExecuteScalar();
                return insertedId;
            }
        }
    }
}
