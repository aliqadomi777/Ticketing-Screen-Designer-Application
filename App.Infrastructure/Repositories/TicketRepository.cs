using App.Domain.Interfaces;
using App.Domain.Models;
using App.Shared;
using System;
using System.Data;
using System.Data.SqlClient;
namespace App.Infrastructure.Repositories
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
                INSERT INTO Tickets (ButtonID, ServiceID) 
                VALUES (@ButtonID, @ServiceID);
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
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new DuplicateRecordException(
                    $"A Ticket Already exists for this button",
                    ex);
            }

            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new DuplicateRecordException(
                    $"The Button your adding Ticket to has been deleted",
                    ex);
            }

        }
        public bool Update(int newServiceId, int ticketId)
        {
            string query = @"
            UPDATE Tickets 
            SET ServiceID=@ServiceID 
            WHERE TicketID=@TicketID;";

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
        public bool Delete(int buttonId)
        {
            string query = @"
                DELETE 
                FROM Tickets 
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
