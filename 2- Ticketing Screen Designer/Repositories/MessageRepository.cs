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
            catch (SqlException ex)
            {
                Log.Error(ex, "Failed database operation inside MessageRepository.Add for model: {@messageModel} ", messageModel);
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new DuplicateRecordException($"A Message already exists for the same Button with ID {messageModel.ButtonId}. ", ex);
                }
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in MessageRepository.Add");
                throw;
            }
        }

        public bool Update(MessageModel messageModel)
        {
            string query = @"
            UPDATE Messages 
            SET MessageEN=@MessageEN, MessageAR=@MessageAR
            WHERE MessageID=@MessageID;";
            try
            {
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
            catch (SqlException ex)
            {
                Log.Error(ex, "Failed database operation inside MessageRepository.Update for model: {@messageModel} ", messageModel);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in MessageRepository.Update");
                throw;
            }


        }
        public bool Delete(int buttonId)
        {
            string query = @"DELETE FROM Messages WHERE ButtonID = @ButtonID;";
            try
            {

                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ButtonID", SqlDbType.Int).Value = buttonId;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "Failed database operation inside MessageRepository.Delete model by ID: {buttonId} ", buttonId);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in MessageRepository.Delete");
                throw;
            }
        }



    }
}
