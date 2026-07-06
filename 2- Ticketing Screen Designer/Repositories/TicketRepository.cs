using System;
using System.Data;
using System.Data.SqlClient;
using Ticketing_Screen_Designer.Interfaces;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Repositories;

namespace Ticketing_Screen_Designer
{
    public class TicketRepository : BaseRepository,
        IDeleteableRepository<TicketModel>,
        IAddableRepository<TicketModel>,
        ITicketRepository<TicketModel>
    {
        public TicketRepository(string connectionString) : base(connectionString) { }

        public int Add(TicketModel model)
        {
            string query = @"
            INSERT INTO Tickets (ButtonID, ServiceID) VALUES (@ButtonID, @ServiceID);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ServiceID", SqlDbType.Int).Value = model.ServiceId;
                    cmd.Parameters.Add("@ButtonID", SqlDbType.Int).Value = model.ButtonId;
                    conn.Open();
                    if (cmd.ExecuteScalar() is int newId)
                    {
                        return newId;
                    }
                    throw new Exception();

                }
            }
        }
        public bool Update(int serviceId, TicketModel model)
        {
            string query = @"
            UPDATE Tickets 
            SET ServiceID=@ServiceID
            WHERE TicketID=@TicketID;";
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@TicketID", SqlDbType.NVarChar, 100).Value = model.TicketId;
                    cmd.Parameters.Add("@ServiceID", SqlDbType.NVarChar, 100).Value = model.ServiceId;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        public bool Delete(int id)
        {
            string query = @"DELETE FROM Tickets WHERE TicketID = @TicketID";

            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@TicketID", SqlDbType.Int).Value = id;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
