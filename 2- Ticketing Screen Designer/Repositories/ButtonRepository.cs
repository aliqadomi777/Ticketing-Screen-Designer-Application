using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Utils;
namespace Ticketing_Screen_Designer.Repositories
{
    public class ButtonRepository : BaseRepository,
        IButtonRepository<ButtonModel>,
        IAddableRepository<ButtonModel>,
        IAddableRepository<MessageModel>,
        IAddableRepository<TicketModel>,
        IDeleteableRepository<ButtonModel>,
        IListableRepository<ButtonModel>,
        IUpdateableRepository<ButtonModel>
    {
        public ButtonRepository(string connectionString) : base(connectionString) { }
        public ButtonModel GetById(int buttonId, int buttonType)
        {
            string query = string.Empty;
            if (buttonType == 1)
            {
                query = @"
                SELECT B.ButtonID, B.ButtonNameEN, B.ButtonNameAR, B.ScreenID, B.ModifiedAt, B.ButtonType, T.TicketID, T.ServiceID, S.ServicesName
                FROM Buttons B INNER JOIN Tickets T ON B.ButtonID = T.ButtonID
                INNER JOIN Services S ON S.ServiceID = T.ServiceID
                WHERE B.ButtonID = @ButtonID;";

            }
            else if (buttonType == 2)
            {
                query = @"
                SELECT B.ButtonID,B.ButtonNameEN, B.ButtonNameAR, B.ScreenID, B.ModifiedAt, B.ButtonType, M.MessageID, M.MessageEN, M.MessageAR
                FROM Buttons B INNER JOIN Messages M ON B.ButtonID = M.ButtonID
                WHERE B.ButtonID = @ButtonID;";
            }
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ButtonID", SqlDbType.Int).Value = buttonId;
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (buttonType == 2)
                            {
                                return new MessageModel
                                {
                                    ButtonId = reader.GetInt32(reader.GetOrdinal("ButtonID")),
                                    ButtonNameAR = reader.GetString(reader.GetOrdinal("ButtonNameAR")),
                                    ButtonNameEN = reader.GetString(reader.GetOrdinal("ButtonNameEN")),
                                    ButtonType = reader.GetInt32(reader.GetOrdinal("ButtonType")),
                                    ModifiedAt = reader.GetDateTimeOffset(reader.GetOrdinal("ModifiedAt")),
                                    ScreenId = reader.GetInt32(reader.GetOrdinal("ScreenID")),
                                    MessageId = reader.GetInt32(reader.GetOrdinal("MessageID")),
                                    MessageEN = reader.GetString(reader.GetOrdinal("MessageEN")),
                                    MessageAR = reader.GetString(reader.GetOrdinal("MessageAR")),
                                    TypeName = "Show Message",

                                };
                            }
                            else if (buttonType == 1)
                            {
                                return new TicketModel
                                {
                                    ButtonId = reader.GetInt32(reader.GetOrdinal("ButtonID")),
                                    ButtonNameAR = reader.GetString(reader.GetOrdinal("ButtonNameAR")),
                                    ButtonNameEN = reader.GetString(reader.GetOrdinal("ButtonNameEN")),
                                    ButtonType = reader.GetInt32(reader.GetOrdinal("ButtonType")),
                                    ModifiedAt = reader.GetDateTimeOffset(reader.GetOrdinal("ModifiedAt")),
                                    ScreenId = reader.GetInt32(reader.GetOrdinal("ScreenID")),
                                    ServiceId = reader.GetInt32(reader.GetOrdinal("ServiceID")),
                                    ServiceName = reader.GetString(reader.GetOrdinal("ServicesName")),
                                    TicketId = reader.GetInt32(reader.GetOrdinal("TicketID")),
                                    TypeName = "Issue Ticket",

                                };
                            }

                        }
                    }
                }


                return null;
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "Failed database operation inside ButtonRepository.GetById for ID: {buttonId} ", buttonId);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in ButtonRepository.GetById for ID: {buttonId} ", buttonId);
                throw;
            }
        }


        public IEnumerable<ButtonModel> GetAll(int screenId)
        {
            string query = @"
            SELECT B.ButtonID, B.ButtonNameEN, B.ButtonNameAR, B.ButtonType, B.ScreenID, B.ModifiedAt, BT.TypeName
            FROM Buttons B INNER JOIN ButtonTypes BT ON B.ButtonType = BT.TypeID
            WHERE B.ScreenID = @ScreenID;";

            List<ButtonModel> buttons = new List<ButtonModel>();

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = screenId;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        int buttonIdOrd = reader.GetOrdinal("ButtonID");

                        if (reader.HasRows)
                        {
                            int buttonNameENOrd = reader.GetOrdinal("ButtonNameEN");
                            int buttonNameAROrd = reader.GetOrdinal("ButtonNameAR");
                            int buttonTypeOrd = reader.GetOrdinal("ButtonType");
                            int screenIdOrd = reader.GetOrdinal("ScreenID");
                            int modifiedAtOrd = reader.GetOrdinal("ModifiedAt");
                            int typeNameOrd = reader.GetOrdinal("TypeName");
                            while (reader.Read())
                            {
                                buttons.Add(new ButtonModel
                                {
                                    ButtonId = reader.GetInt32(buttonIdOrd),
                                    ButtonNameEN = reader.GetString(buttonNameENOrd),
                                    ButtonNameAR = reader.GetString(buttonNameAROrd),
                                    ButtonType = reader.GetInt32(buttonTypeOrd),
                                    ScreenId = reader.GetInt32(screenIdOrd),
                                    ModifiedAt = reader.GetDateTimeOffset(modifiedAtOrd),
                                    TypeName = reader.GetString(typeNameOrd),
                                });
                            }
                        }


                    }

                }

                return buttons;
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "Failed database operation inside ButtonRepository.GetAll for ID: {screenId} ", screenId);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in ButtonRepository.GetAll for ID: {screenId} ", screenId);
                throw;
            }

        }

        public int Add(ButtonModel buttonModel)
        {
            string query = @"
            INSERT INTO Buttons (ButtonNameEN, ButtonNameAR, ButtonType, ScreenID) 
            VALUES (@ButtonNameEN, @ButtonNameAR, @ButtonType, @ScreenID);
            SELECT CAST(SCOPE_IDENTITY() as int);";
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = buttonModel.ScreenId;
                    cmd.Parameters.Add("@ButtonNameAR", SqlDbType.NVarChar, 100).Value = buttonModel.ButtonNameAR;
                    cmd.Parameters.Add("@ButtonNameEN", SqlDbType.NVarChar, 100).Value = buttonModel.ButtonNameEN;
                    cmd.Parameters.Add("@ButtonType", SqlDbType.Int).Value = buttonModel.ButtonType;
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
                Log.Error(ex, "Failed database operation inside ButtonRepository.Add for model: {@buttonModel} ", buttonModel);
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new DuplicateRecordException($"A button with the same naming already exist on screen for ID {buttonModel.ScreenId}", ex);
                }
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in ButtonRepository.Add");
                throw;
            }
        }
        public int Add(TicketModel ticketModel)
        {
            string query = @"
            INSERT INTO Buttons(ButtonNameEN, ButtonNameAR, ButtonType, ScreenID)
            VALUES(@ButtonNameEN, @ButtonNameAR, @ButtonType, @ScreenID);

            DECLARE @NewButtonID INT = SCOPE_IDENTITY();

            INSERT INTO Tickets(ButtonID, ServiceID)
            VALUES(@NewButtonID, @ServiceID);

            SELECT @NewButtonID; ";

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                using (var cmd = new SqlCommand(query, conn, transaction))
                {
                    cmd.Parameters.Add("@ButtonNameEN", SqlDbType.NVarChar, 100).Value = ticketModel.ButtonNameEN;
                    cmd.Parameters.Add("@ButtonNameAR", SqlDbType.NVarChar, 100).Value = ticketModel.ButtonNameAR;
                    cmd.Parameters.Add("@ButtonType", SqlDbType.Int).Value = ticketModel.ButtonType;
                    cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = ticketModel.ScreenId;
                    cmd.Parameters.Add("@ServiceID", SqlDbType.Int).Value = ticketModel.ServiceId;
                    try
                    {
                        int generatedId = Convert.ToInt32(cmd.ExecuteScalar());
                        transaction.Commit();

                        return generatedId;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new InvalidOperationException("", e);

                    }
                }

            }
        }

        public int Add(MessageModel messageModel)
        {
            string query = @"
            INSERT INTO Buttons (ButtonNameEN, ButtonNameAR, ButtonType, ScreenID) 
            VALUES (@ButtonNameEN, @ButtonNameAR, @ButtonType, @ScreenID);

            DECLARE @NewButtonID INT = SCOPE_IDENTITY();

            INSERT INTO Messages (MessageEN, MessageAR, ButtonID) 
            VALUES (@MessageEN, @MessageAR, @NewButtonID);

            SELECT @NewButtonID;";

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                using (var cmd = new SqlCommand(query, conn, transaction))
                {
                    cmd.Parameters.Add("@ButtonNameEN", SqlDbType.NVarChar, 100).Value = messageModel.ButtonNameEN;
                    cmd.Parameters.Add("@ButtonNameAR", SqlDbType.NVarChar, 100).Value = messageModel.ButtonNameAR;
                    cmd.Parameters.Add("@ButtonType", SqlDbType.Int).Value = messageModel.ButtonType;
                    cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = messageModel.ScreenId;
                    cmd.Parameters.Add("@MessageEN", SqlDbType.NVarChar, 500).Value = messageModel.MessageEN;
                    cmd.Parameters.Add("@MessageAR", SqlDbType.NVarChar, 500).Value = messageModel.MessageAR;

                    try
                    {

                        int generatedId = Convert.ToInt32(cmd.ExecuteScalar());
                        transaction.Commit();

                        return generatedId;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new InvalidOperationException("", e);
                    }
                }

            }
        }

        public bool Update(ButtonModel buttonModel)
        {
            string query = @"
            UPDATE Buttons 
            SET ButtonNameEN=@ButtonNameEN, ButtonNameAR=@ButtonNameAR, ButtonType=@ButtonType
            WHERE ButtonID=@ButtonID;";
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ButtonNameEN", SqlDbType.NVarChar, 100).Value = buttonModel.ButtonNameEN;
                    cmd.Parameters.Add("@ButtonNameAR", SqlDbType.NVarChar, 100).Value = buttonModel.ButtonNameAR;
                    cmd.Parameters.Add("@ButtonType", SqlDbType.Int).Value = buttonModel.ButtonType;
                    cmd.Parameters.Add("@ButtonID", SqlDbType.Int).Value = buttonModel.ButtonId;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "Failed database operation inside ButtonRepository.Update for model: {@buttonModel} ", buttonModel);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in ButtonRepository.Update");
                throw;
            }

        }
        public bool Delete(int buttonId)
        {
            string query = @"DELETE FROM Buttons WHERE ButtonID = @ButtonID;";

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
                Log.Error(ex, "Failed database operation inside ButtonRepository.Delete model by ID: {buttonId} ", buttonId);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in ButtonRepository.Delete");
                throw;
            }

        }

    }
}