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
    public class ScreenRepository : BaseRepository,
        IFetchableRepository<ScreenModel>,
        IAddableRepository<ScreenModel>,
        IDeleteableRepository<ScreenModel>,
        IListableRepository<ScreenModel>,
        IUpdateableRepository<ScreenModel>
    {
        public ScreenRepository(string connectionString) : base(connectionString) { }

        public ScreenModel GetById(int screenId)
        {
            string query = @"SELECT ScreenID, ScreenName, IsActive, ModifiedAt, BankID FROM Screens WHERE ScreenID = @ScreenID;";
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = screenId;
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
                return null;
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "Failed database operation inside ScreenRepository.GetById for ID: {screenId} ", screenId);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in ScreenRepository.GetById for ID: {screenId} ", screenId);
                throw;
            }
        }
        public IEnumerable<ScreenModel> GetAll(int bankId)
        {
            string query = @"SELECT ScreenID, ScreenName, IsActive, ModifiedAt, BankID FROM Screens WHERE BankID = @BankID;";
            List<ScreenModel> screens = new List<ScreenModel>();
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = bankId;
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
            catch (SqlException ex)
            {
                Log.Error(ex, "Failed database operation inside ScreenRepository.GetAll for ID: {bankId} ", bankId);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in ScreenRepository.GetAll for ID: {bankId} ", bankId);
                throw;
            }

        }

        public int Add(ScreenModel screenModel)
        {

            string deactivateQuery = @"UPDATE Screens SET IsActive=0 WHERE BankID=@BankID;";
            string query = @"
            INSERT INTO Screens (ScreenName, IsActive, BankID) VALUES(@ScreenName, @IsActive, @BankID);
            SELECT CAST(SCOPE_IDENTITY() as int);";
            if (screenModel.IsActive)
            {
                query = deactivateQuery + query;
            }

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    using (var cmd = new SqlCommand(query, conn, transaction))
                    {
                        cmd.Parameters.Add("@ScreenName", SqlDbType.NVarChar, 100).Value = screenModel.ScreenName;
                        cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = screenModel.BankId;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = screenModel.IsActive;
                        if (cmd.ExecuteScalar() is int newId)
                        {
                            transaction.Commit();
                            return newId;
                        }
                        throw new InvalidOperationException("Database failed to return a valid identity ID.");

                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "Failed database operation inside ScreenRepository.Add for model: {@screenModel} ", screenModel);
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new DuplicateRecordException($"A Screen named {screenModel.ScreenName} already exists for the same Bank. ", ex);
                }
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in ScreenRepository.Add");
                throw;
            }

        }
        public bool Update(ScreenModel screenModel)
        {
            string deactivateQuery = @"UPDATE Screens SET IsActive=0 WHERE BankID=@BankID AND ScreenID!=@ScreenID;";
            string query = @"UPDATE Screens SET ScreenName=@ScreenName, IsActive=@IsActive WHERE ScreenID=@ScreenID";
            if (screenModel.IsActive)
            {
                query = deactivateQuery + query;
            }
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    using (var cmd = new SqlCommand(query, conn, transaction))
                    {
                        cmd.Parameters.Add("@ScreenName", SqlDbType.NVarChar, 100).Value = screenModel.ScreenName;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = screenModel.IsActive;
                        cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = screenModel.ScreenId;
                        cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = screenModel.BankId;

                        int rowsAffected = cmd.ExecuteNonQuery();
                        transaction.Commit();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "Failed database operation inside ScreenRepository.Update for model: {@screenModel} ", screenModel);
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    throw new ExcessiveScreenActivationException("Another screen is already active for this bank.", ex);
                }
                throw;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in ScreenRepository.Update");
                throw;
            }
        }




        public bool Delete(int screenId)
        {
            string query = @"DELETE FROM Screens WHERE ScreenID = @ScreenID;";
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = screenId;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "Failed database operation inside ScreenRepository.Delete model by ID: {@screenId} ", screenId);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in ScreenRepository.Delete");
                throw;
            }


        }


    }
}