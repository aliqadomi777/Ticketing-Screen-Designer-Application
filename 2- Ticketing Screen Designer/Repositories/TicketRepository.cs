using Serilog;
using System;
using System.Data;
using System.Data.SqlClient;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Utils;
namespace Ticketing_Screen_Designer.Repositories
{
    public class TicketRepository : BaseRepository,
        IDeleteableRepository<TicketModel>,
        IAddableRepository<TicketModel>,
        ITicketRepository<TicketModel>
    {
        public TicketRepository(string connectionString) : base(connectionString) { }

        public int Add(TicketModel ticketModel)
        {
            string query = @"
            INSERT INTO Tickets (ButtonID, ServiceID) VALUES (@ButtonID, @ServiceID);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ServiceID", SqlDbType.Int).Value = ticketModel.ServiceId;
                    cmd.Parameters.Add("@ButtonID", SqlDbType.Int).Value = ticketModel.ButtonId;
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
                Log.Error(ex, "Failed database operation inside TicketRepository.Add for model: {@ticketModel} ", ticketModel);
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new DuplicateRecordException($"A Ticket for the same button already exists", ex);
                }
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in TicketRepository.Add");
                throw;
            }

        }
        public bool Update(int newServiceId, int ticketId)
        {
            string query = @"
            UPDATE Tickets SET ServiceID=@ServiceID WHERE TicketID=@TicketID;";
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@TicketID", SqlDbType.Int).Value = ticketId;
                    cmd.Parameters.Add("@ServiceID", SqlDbType.Int).Value = newServiceId;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "Failed database operation inside TicketRepository.Update ticket by ID: {ticketId} change service ID to {newServiceId}", ticketId, newServiceId);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in TicketRepository.Update");
                throw;
            }

        }
        public bool Delete(int buttonId)
        {
            string query = @"DELETE FROM Tickets WHERE ButtonID = @ButtonID;";

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
                Log.Error(ex, "Failed database operation inside TicketRepository.Delete model by ID: {buttonId} ", buttonId);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in TicketRepository.Delete");
                throw;
            }

        }
    }
}
