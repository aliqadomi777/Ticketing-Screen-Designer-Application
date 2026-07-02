using System;
using System.Data;
using System.Data.SqlClient;
using Ticketing_Screen_Designer.Interfaces;
using Ticketing_Screen_Designer.Models;
namespace Ticketing_Screen_Designer.Repositories
{
    public class BankRepository : BaseRepository,
        IFetchableRepository<BankModel>,
        IAddableRepository<BankModel>

    {
        public BankRepository(string connectionString) : base(connectionString) { }

        public BankModel GetById(int id)
        {
            string query = @"SELECT BankID, BankName FROM Banks WHERE BankID = @BankID";
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = id;
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        int bankIdOrd = reader.GetOrdinal("BankID");
                        int bankNameOrd = reader.GetOrdinal("BankName");
                        if (reader.Read())
                        {
                            return new BankModel
                            {
                                BankId = reader.GetInt32(bankIdOrd),
                                BankName = reader.GetString(bankNameOrd)
                            };
                        }
                    }
                }

            }
            return null;
        }
        public int Add(BankModel model)
        {

            ValidateModel(model);
            string query = @"
            INSERT INTO Banks (BankName) VALUES (@BankName);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.Add("@BankName", SqlDbType.NVarChar, 100).Value = model.BankName;
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return (int)result;
                }
            }


        }
    }
}

