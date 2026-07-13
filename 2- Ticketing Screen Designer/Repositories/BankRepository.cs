using Serilog;
using System;
using System.Data;
using System.Data.SqlClient;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Utils;
namespace Ticketing_Screen_Designer.Repositories
{
    public class BankRepository : BaseRepository,
        IFetchableRepository<BankModel>,
        IAddableRepository<BankModel>

    {
        public BankRepository(string connectionString) : base(connectionString) { }

        public BankModel GetById(int bankId)
        {
            string query = @"SELECT BankID, BankName FROM Banks WHERE BankID = @BankID;";

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = bankId;
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return new BankModel
                            {
                                BankId = reader.GetInt32(reader.GetOrdinal("BankID")),
                                BankName = reader.GetString(reader.GetOrdinal("BankName"))
                            };
                        }


                    }
                }
                return null;
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "Failed database operation inside BankRepository.GetById for ID: {bankId} ", bankId);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in BankRepository.GetById for ID: {bankId} ", bankId);
                throw;
            }


        }
        public int Add(BankModel bankModel)
        {
            string query = @"
            INSERT INTO Banks (BankName) VALUES (@BankName);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@BankName", SqlDbType.NVarChar, 100).Value = bankModel.BankName;
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
                Log.Error(ex, "Failed database operation inside BankRepository.Add for model: {@bankModel} ", bankModel);
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new DuplicateRecordException($"A bank named '{bankModel.BankName}' already exists. ", ex);
                }
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Critical, unexpected system error in BankRepository.Add");
                throw;
            }
        }
    }


}



