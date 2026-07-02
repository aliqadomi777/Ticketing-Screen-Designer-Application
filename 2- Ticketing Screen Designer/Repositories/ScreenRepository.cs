using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ticketing_Screen_Designer.Interfaces;
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
                        int screenIdOrd = reader.GetOrdinal("ScreenID");
                        int screenNameOrd = reader.GetOrdinal("ScreenName");
                        int isActiveOrd = reader.GetOrdinal("IsActive");
                        int modifiedAtOrd = reader.GetOrdinal("ModifiedAt");
                        int bankIdOrd = reader.GetOrdinal("BankID");

                        if (reader.Read())
                        {
                            return new ScreenModel
                            {
                                ScreenId = reader.GetInt32(screenIdOrd),
                                ScreenName = reader.GetString(screenNameOrd),
                                IsActive = reader.GetBoolean(isActiveOrd),
                                ModifiedAt = reader.GetDateTimeOffset(modifiedAtOrd),
                                BankId = reader.GetInt32(bankIdOrd)
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
            return screens.Count == 0 ? null : screens;

        }

        public int Add(ScreenModel model)
        {
            ValidateModel(model);
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
                    var result = cmd.ExecuteScalar();
                    return (int)result;
                }
            }
        }
        public void Update(ScreenModel model)
        {
            ValidateModel(model);
            string query = @"UPDATE Screens SET ScreenName=@ScreenName, IsActive=@IsActive, ModifiedAt=@ModifiedAt WHERE ScreenID=@ScreenID";
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ScreenName", SqlDbType.NVarChar, 100).Value = model.ScreenName;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = model.IsActive;
                    cmd.Parameters.Add("@ModifiedAt", SqlDbType.DateTimeOffset).Value = model.ModifiedAt;
                    cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = model.ScreenId;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }


            }
        }
        public void Delete(int id)
        {
            string query = @"DELETE FROM Screens WHERE ScreenID = @ScreenID";
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ScreenID", SqlDbType.Int).Value = id;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}