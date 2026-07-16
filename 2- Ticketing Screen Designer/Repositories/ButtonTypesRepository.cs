using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Models;
namespace Ticketing_Screen_Designer.Repositories
{
    public class ButtonTypeRepository : BaseRepository,
        IFetchableRepository<ButtonTypes>,
        IGetAllRepository<ButtonTypes>
    {
        public ButtonTypeRepository(string connectionString) : base(connectionString) { }
        public ButtonTypes GetById(int typeId)
        {
            string query = @"
                SELECT TypeID, TypeName 
                FROM ButtonTypes 
                WHERE TypeID = @TypeID;";

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@TypeID", SqlDbType.Int).Value = typeId;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ButtonTypes
                        {
                            TypeId = reader.GetInt32(reader.GetOrdinal("TypeID")),
                            TypeName = reader.GetString(reader.GetOrdinal("TypeName")),

                        };
                    }
                }
            }

            return null;
        }



        public IEnumerable<ButtonTypes> GetAll()
        {
            string query = @"
                SELECT TypeID, TypeName 
                FROM ButtonTypes;";
            List<ButtonTypes> buttonTypes = new List<ButtonTypes>();

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
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


            return buttonTypes;

        }
    }
}