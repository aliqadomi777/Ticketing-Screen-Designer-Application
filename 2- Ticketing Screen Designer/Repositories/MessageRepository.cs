using System;
using System.Data;
using System.Data.SqlClient;
using Ticketing_Screen_Designer.Interfaces;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Repositories;

namespace Ticketing_Screen_Designer
{
    public class MessageRepository : BaseRepository,
        IDeleteableRepository<MessageModel>,
        IAddableRepository<MessageModel>,
        ITicketRepository<MessageModel>
    {
        public MessageRepository(string connectionString) : base(connectionString) { }
        public int Add(MessageModel model)
        {
            string query = @"
            INSERT INTO Messages (ButtonID, MessageEN, MessageAR) 
            VALUES (@ButtonID, @MessageEN, @MessageAR);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ButtonID", SqlDbType.Int).Value = model.ButtonId;
                    cmd.Parameters.Add("@MessageEN", SqlDbType.NVarChar, 500).Value = model.MessageEN;
                    cmd.Parameters.Add("@MessageAR", SqlDbType.NVarChar, 500).Value = model.MessageAR;
                    conn.Open();
                    if (cmd.ExecuteScalar() is int newId)
                    {
                        return newId;
                    }
                    throw new Exception();

                }
            }
        }
        public bool Update(int serviceId, MessageModel model)
        {
            string query = @"
            UPDATE Messages 
            SET MessageEN=@MessageEN, MessageAR=@MessageAR
            WHERE MessageID=@MessageID;";
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MessageID", SqlDbType.Int).Value = model.ButtonId;
                    cmd.Parameters.Add("@MessageEN", SqlDbType.NVarChar, 500).Value = model.MessageEN;
                    cmd.Parameters.Add("@MessageAR", SqlDbType.NVarChar, 500).Value = model.MessageAR;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        public bool Delete(int id)
        {
            string query = @"DELETE FROM Messages WHERE MessageID = @MessageID";

            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MessageID", SqlDbType.Int).Value = id;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }


    }
}
