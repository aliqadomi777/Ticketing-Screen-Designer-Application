using App.Domain.Interfaces;
using App.Domain.Models;
using App.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace App.Infrastructure.Repositories
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
            string query = @"
            
                SELECT ScreenID, ScreenName, IsActive, ModifiedAt, BankID 
                FROM Screens 
                WHERE ScreenID = @ScreenID;";

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
        public IEnumerable<ScreenModel> GetAll(int bankId)
        {
            string query = @"

                SELECT ScreenID, ScreenName, IsActive, ModifiedAt, BankID 
                FROM Screens 
                WHERE BankID = @BankID;";

            List<ScreenModel> screens = new List<ScreenModel>();

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

        public int Add(ScreenModel screenModel)
        {

            //string deactivateQuery = @"
            //    UPDATE Screens 
            //    SET IsActive=0 
            //    WHERE BankID=@BankID;";

            string query = @"
                INSERT INTO Screens (ScreenName, IsActive, BankID)
                VALUES(@ScreenName, @IsActive, @BankID);
                SELECT CAST(SCOPE_IDENTITY() as int);";
            //if (screenModel.IsActive)
            //{
            //    query = deactivateQuery + query;
            //}

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
            catch (SqlException ex) when (ex.Number == 2627)
            {
                throw new DuplicateRecordException(
                    $"A Screen named '{screenModel.ScreenName}' already exists.",
                    ex);
            }
            catch (SqlException ex) when (ex.Number == 2601)
            {
                throw new ExcessiveScreenActivationException(
                    $"Another screen is already Active for the bank",
                    ex);
            }

            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new ParentDeletedWithChildConflictException(
                    $"The bank your adding screen to has been deleted",
                    ex);
            }


        }
        public bool Update(ScreenModel screenModel)
        {
            //string deactivateQuery = @"UPDATE Screens SET IsActive=0 WHERE BankID=@BankID AND ScreenID!=@ScreenID;";

            string query = @"
                UPDATE Screens 
                SET ScreenName=@ScreenName, IsActive=@IsActive 
                WHERE ScreenID=@ScreenID;";

            //if (screenModel.IsActive)
            //{
            //    query = deactivateQuery + query;
            //}
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
            catch (SqlException ex) when (ex.Number == 2627)
            {
                throw new DuplicateRecordException(
                    $"A Screen named '{screenModel.ScreenName}' already exists.",
                    ex);
            }
            catch (SqlException ex) when (ex.Number == 2601)
            {
                throw new ExcessiveScreenActivationException(
                    $"Another screen is already Active for the bank",
                    ex);
            }


        }




        public bool Delete(int screenId)
        {
            string query = @"
                DELETE 
                FROM Screens 
                WHERE ScreenID = @ScreenID;";

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = screenId;
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }

        }


    }
}