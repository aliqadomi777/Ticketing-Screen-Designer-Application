using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ticketing_Screen_Designer.Interfaces;
using Ticketing_Screen_Designer.Models;

namespace Ticketing_Screen_Designer.Repositories
{
    public class ButtonRepository : BaseRepository,
        IButtonRepository<ButtonModel>,
        IAddableRepository<MessageModel>,
        IAddableRepository<TicketModel>,
        IDeleteableRepository<ButtonModel>,
        IListableRepository<ButtonModel>,
        IUpdateableRepository<ButtonModel>
    {
        public ButtonRepository(string connectionString) : base(connectionString) { }
        public ButtonModel GetById(int id, string typeName)
        {
            string query = string.Empty;
            if (typeName == "Issue Ticket")
            {
                query = @"
                SELECT B.ButtonID, B.ButtonNameEN, B.ButtonNameAR, B.ScreenID, B.ModifiedAt, B.ButtonType, T.TicketID, T.ServiceID, S.ServicesName
                FROM Buttons B INNER JOIN Tickets T ON B.ButtonID = T.ButtonID
                INNER JOIN Services S ON S.ServiceID = T.ServiceID
                WHERE B.ButtonID = @ButtonID;";

            }
            else if (typeName == "Show Message")
            {
                query = @"
                SELECT B.ButtonID,B.ButtonNameEN, B.ButtonNameAR, B.ScreenID, B.ModifiedAt, B.ButtonType, M.MessageID, M.MessageEN, M.MessageAR
                FROM Buttons B INNER JOIN Messages M ON B.ButtonID = M.ButtonID
                WHERE B.ButtonID = @ButtonID;";
            }
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ButtonID", SqlDbType.Int).Value = id;
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (typeName == "Show Message")
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
                            else if (typeName == "Issue Ticket")
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

            }
            return null;
        }
        public IEnumerable<ButtonModel> GetAll(int id)
        {
            string query = @"
            SELECT B.ButtonID, B.ButtonNameEN, B.ButtonNameAR, B.ButtonType, B.ScreenID, B.ModifiedAt, BT.TypeName
            FROM Buttons B INNER JOIN ButtonTypes BT ON B.ButtonType = BT.TypeID
            WHERE B.ScreenID = @ScreenID ";

            List<ButtonModel> buttons = new List<ButtonModel>();

            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = id;
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
            }
            return buttons;

        }

        public int Add(TicketModel model)
        {
            ValidateModel(model);
            string query = @"
            INSERT INTO Buttons (ButtonNameEN, ButtonNameAR, ButtonType, ScreenID) 
            VALUES (@ButtonNameEN, @ButtonNameAR, @ButtonType, @ScreenID);

            DECLARE @NewButtonID INT = SCOPE_IDENTITY();

            INSERT INTO Tickets (ButtonID, ServiceID) 
            VALUES (@NewButtonID, @ServiceID);

            SELECT @NewButtonID;";

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    using (var cmd = new SqlCommand(query, conn, transaction))
                    {
                        cmd.Parameters.Add("@ButtonNameEN", SqlDbType.NVarChar, 100).Value = model.ButtonNameEN;
                        cmd.Parameters.Add("@ButtonNameAR", SqlDbType.NVarChar, 100).Value = model.ButtonNameAR;
                        cmd.Parameters.Add("@ButtonType", SqlDbType.Int).Value = model.ButtonType;
                        cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = model.ScreenId;
                        cmd.Parameters.Add("@ServiceID", SqlDbType.Int).Value = model.ServiceId;
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
        }

        public int Add(MessageModel model)
        {
            ValidateModel(model);
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
                {
                    using (var cmd = new SqlCommand(query, conn, transaction))
                    {
                        cmd.Parameters.Add("@ButtonNameEN", SqlDbType.NVarChar, 100).Value = model.ButtonNameEN;
                        cmd.Parameters.Add("@ButtonNameAR", SqlDbType.NVarChar, 100).Value = model.ButtonNameAR;
                        cmd.Parameters.Add("@ButtonType", SqlDbType.Int).Value = model.ButtonType;
                        cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = model.ScreenId;
                        cmd.Parameters.Add("@MessageEN", SqlDbType.NVarChar, 500).Value = model.MessageEN;
                        cmd.Parameters.Add("@MessageAR", SqlDbType.NVarChar, 500).Value = model.MessageAR;

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
        }

        public bool Update(ButtonModel model)
        {
            ValidateModel(model);
            string query = @"
            UPDATE Buttons 
            SET ButtonNameEN=@ButtonNameEN, ButtonNameAR=@ButtonNameAR, ButtonType=@ButtonType, ModifiedAt=SYSUTCDATETIME()
            WHERE ButtonID=@ButtonID;";
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ButtonNameEN", SqlDbType.NVarChar, 100).Value = model.ButtonNameEN;
                    cmd.Parameters.Add("@ButtonNameAR", SqlDbType.NVarChar, 100).Value = model.ButtonNameAR;
                    cmd.Parameters.Add("@ButtonType", SqlDbType.Int).Value = model.ButtonType;
                    cmd.Parameters.Add("@ButtonID", SqlDbType.Int).Value = model.ButtonId;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        public bool Delete(int id)
        {
            string query = @"DELETE FROM Buttons WHERE ButtonID = @ButtonID";

            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ButtonID", SqlDbType.Int).Value = id;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

    }
}