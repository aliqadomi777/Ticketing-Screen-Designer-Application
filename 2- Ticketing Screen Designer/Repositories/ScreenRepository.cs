using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Models;
namespace Ticketing_Screen_Designer.Repositories
{
    public class ScreenRepository : BaseRepository,
        IFetchableRepository<ScreenModel>,
        IAddableRepository<ScreenModel>,
        IDeleteableRepository<ScreenModel>,
        IListableRepository<ScreenModel>,
        IUpdateableRepository<ScreenModel>
    {
        public ScreenRepository(string connectionString) : base(connectionString) { }

        public ScreenModel GetById(int id)
        {
            string query = @"SELECT ScreenID, ScreenName, IsActive, ModifiedAt, BankID FROM Screens WHERE ScreenID = @ScreenID;";
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = id;
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return new ScreenModel
                            {
                                ScreenId = reader.GetInt32(reader.GetOrdinal("ScreenID")),
                                ScreenName = reader.GetString(reader.GetOrdinal("ScreenName")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                ModifiedAt = reader.GetDateTimeOffset(reader.GetOrdinal("ModifiedAt")),
                                BankId = reader.GetInt32(reader.GetOrdinal("BankID"))
                            };
                        }
                    }
                }
            }
            return null;
        }
        public IEnumerable<ScreenModel> GetAll(int id)
        {
            string query = @"SELECT ScreenID, ScreenName, IsActive, ModifiedAt, BankID FROM Screens WHERE BankID = @BankID;";
            List<ScreenModel> screens = new List<ScreenModel>();
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {

                cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = id;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        int screenIdOrd = reader.GetOrdinal("ScreenID");
                        int screenNameOrd = reader.GetOrdinal("ScreenName");
                        int isActiveOrd = reader.GetOrdinal("IsActive");
                        int modifiedAtOrd = reader.GetOrdinal("ModifiedAt");
                        int bankIdOrd = reader.GetOrdinal("BankID");
                        while (reader.Read())
                        {
                            screens.Add(new ScreenModel
                            {
                                ScreenId = reader.GetInt32(screenIdOrd),
                                ScreenName = reader.GetString(screenNameOrd),
                                IsActive = reader.GetBoolean(isActiveOrd),
                                ModifiedAt = reader.GetDateTimeOffset(modifiedAtOrd),
                                BankId = reader.GetInt32(bankIdOrd),
                            });
                        }
                    }

                }

            }
            return screens;

        }

        public int Add(ScreenModel model)
        {
            string query = @"
            INSERT INTO Screens (ScreenName, BankID) VALUES(@Name, @BankId);
            SELECT CAST(SCOPE_IDENTITY() as int);";
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = model.ScreenName;
                    cmd.Parameters.Add("@BankId", SqlDbType.Int).Value = model.BankId;
                    conn.Open();
                    if (cmd.ExecuteScalar() is int newId)
                    {
                        return newId;
                    }
                    throw new Exception();

                }
            }
        }
        public bool Update(ScreenModel model)
        {
            string deactivateQuery = @"UPDATE Screens SET IsActive=0 WHERE BankID=@BankID AND ScreenID!=@ScreenID;";
            string query = @"UPDATE Screens SET ScreenName=@ScreenName, IsActive=@IsActive,  ModifiedAt=SYSUTCDATETIME() WHERE ScreenID=@ScreenID";
            if (model.IsActive)
            {
                query = deactivateQuery + query;
            }
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    using (var cmd = new SqlCommand(query, conn, transaction))
                    {
                        cmd.Parameters.Add("@ScreenName", SqlDbType.NVarChar, 100).Value = model.ScreenName;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = model.IsActive;
                        cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = model.ScreenId;
                        cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = model.BankId;

                        try
                        {
                            int rowsAffected = cmd.ExecuteNonQuery();
                            transaction.Commit();
                            return rowsAffected > 0;
                        }
                        catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
                        {
                            throw new InvalidOperationException("Another screen is already active for this bank.", ex);
                        }
                    }
                }

            }
        }

        public bool Delete(int id)
        {
            string query = @"DELETE FROM Screens WHERE ScreenID = @ScreenID";
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = id;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }


    }
}