using App.Domain.Interfaces;
using App.Domain.Models;
using App.Infrastructure.Exceptions;
using App.Shared;
using System;
using System.Data;
using System.Data.SqlClient;


namespace App.Infrastructure.Repositories
{
    public class BankRepository : BaseRepository,
        IFetchableRepository<BankModel>,
        IAddableRepository<BankModel>

    {
        public BankRepository(string connectionString) : base(connectionString) { }

        public BankModel GetById(int bankId)
        {
            string query = @"

                SELECT BankID, BankName
                FROM Banks
                WHERE BankID = @BankID;";


            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = bankId;

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new BankModel
                    {
                        BankId = reader.GetInt32(reader.GetOrdinal("BankID")),
                        BankName = reader.GetString(reader.GetOrdinal("BankName"))
                    };
                }
            }
        }



        public int Add(BankModel bankModel)
        {
            string query = @"

            INSERT INTO Banks (BankName)
            VALUES (@BankName);

            SELECT CAST(SCOPE_IDENTITY() AS int);";


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

                    throw new InvalidOperationException("Database failed to return the generated identity.");
                }
            }
            catch (SqlException ex) when (ex.Number == (int)SqlErrorTypes.UniqueConstraintViolation)
            {
                throw new DuplicateRecordException(
                    $"A bank named '{bankModel.BankName}' already exists.",
                    ex);
            }


        }
    }


}



