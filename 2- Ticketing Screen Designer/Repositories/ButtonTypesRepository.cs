using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ticketing_Screen_Designer.Interfaces;
using Ticketing_Screen_Designer.Models;
namespace Ticketing_Screen_Designer.Repositories
{
    public class ButtonTypeRepository : BaseRepository,
        IFetchableRepository<ButtonTypes>,
        IGetAllRepository<ButtonTypes>
    {
        public ButtonTypeRepository(string connectionString) : base(connectionString) { }
        public ButtonTypes GetById(int id)
        {
            string query = @"SELECT TypeID, TypeName FROM ButtonTypes WHERE TypeID = @TypeID";
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@TypeID", SqlDbType.Int).Value = id;
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        int typeIdOrd = reader.GetOrdinal("TypeID");
                        int typeNameOrd = reader.GetOrdinal("TypeName");
                        if (reader.Read())
                        {
                            return new ButtonTypes
                            {
                                TypeId = reader.GetInt32(typeIdOrd),
                                TypeName = reader.GetString(typeNameOrd),

                            };
                        }
                    }
                }
            }
            return null;

        }
        public IEnumerable<ButtonTypes> GetAll()
        {
            string query = @"SELECT TypeID, TypeName FROM ButtonTypes";
            List<ButtonTypes> buttonTypes = new List<ButtonTypes>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        int typeIdOrd = reader.GetOrdinal("TypeID");
                        int typeNameOrd = reader.GetOrdinal("TypeName");
                        while (reader.Read())
                        {
                            buttonTypes.Add(new ButtonTypes
                            {
                                TypeId = reader.GetInt32(typeIdOrd),
                                TypeName = reader.GetString(typeNameOrd),
                            });
                        }

                    }

                }

            }
            return buttonTypes.Count == 0 ? null : buttonTypes;
        }
    }
}