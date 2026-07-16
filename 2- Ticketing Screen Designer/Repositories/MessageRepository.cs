using Serilog;
using System;
using System.Data;
using System.Data.SqlClient;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Utils;
namespace Ticketing_Screen_Designer.Repositories
{
    public class MessageRepository : BaseRepository,
        IDeleteableRepository<MessageModel>,
        IAddableRepository<MessageModel>,
        IUpdateableRepository<MessageModel>
    {
        public MessageRepository(string connectionString) : base(connectionString) { }
        public int Add(MessageModel messageModel)
        {
            string query = @"
                INSERT INTO Messages (ButtonID, MessageEN, MessageAR) 
                VALUES (@ButtonID, @MessageEN, @MessageAR);
                SELECT CAST(SCOPE_IDENTITY() as int);";
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ButtonID", SqlDbType.Int).Value = messageModel.ButtonId;
                    cmd.Parameters.Add("@MessageEN", SqlDbType.NVarChar, 500).Value = messageModel.MessageEN;
                    cmd.Parameters.Add("@MessageAR", SqlDbType.NVarChar, 500).Value = messageModel.MessageAR;
                    conn.Open();
                    if (cmd.ExecuteScalar() is int newId)
                    {
                        return newId;
                    }
                    throw new InvalidOperationException("Database failed to return a valid identity ID.");

                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new DuplicateRecordException(
                    $"A Message Already exists for this button",
                    ex);
            }

            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new DuplicateRecordException(
                    $"The Button your adding Message to has been deleted",
                    ex);
            }
        }

        public bool Update(MessageModel messageModel)
        {
            string query = @"
                UPDATE Messages 
                SET MessageEN=@MessageEN, MessageAR=@MessageAR
                WHERE MessageID=@MessageID;";

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@MessageID", SqlDbType.Int).Value = messageModel.ButtonId;
                cmd.Parameters.Add("@MessageEN", SqlDbType.NVarChar, 500).Value = messageModel.MessageEN;
                cmd.Parameters.Add("@MessageAR", SqlDbType.NVarChar, 500).Value = messageModel.MessageAR;
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }



        }
        public bool Delete(int buttonId)
        {
            string query = @"
                DELETE FROM Messages 
                WHERE ButtonID = @ButtonID;";


            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@ButtonID", SqlDbType.Int).Value = buttonId;
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }


        }



    }
}
