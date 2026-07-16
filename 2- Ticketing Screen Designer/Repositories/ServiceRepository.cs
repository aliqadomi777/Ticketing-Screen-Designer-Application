using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Models;
namespace Ticketing_Screen_Designer.Repositories
{
    public class ServiceRepository : BaseRepository,
        IFetchableRepository<ServiceType>,
        IGetAllRepository<ServiceType>
    {
        public ServiceRepository(string connectionString) : base(connectionString) { }
        public ServiceType GetById(int serviceId)
        {
            string query = @"
                SELECT ServiceID, ServicesName 
                FROM Services 
                WHERE ServiceID = @ServiceID;";

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@ServiceID", SqlDbType.Int).Value = serviceId;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ServiceType
                        {
                            ServiceId = reader.GetInt32(reader.GetOrdinal("ServiceID")),
                            ServicesName = reader.GetString(reader.GetOrdinal("ServicesName")),

                        };
                    }
                }
            }

            return null;


        }
        public IEnumerable<ServiceType> GetAll()
        {
            string query = @"
                SELECT ServiceID, ServicesName 
                FROM Services;";
            List<ServiceType> services = new List<ServiceType>();

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        int serviceIdOrd = reader.GetOrdinal("ServiceID");
                        int servicesNameOrd = reader.GetOrdinal("ServicesName");
                        while (reader.Read())
                        {
                            services.Add(new ServiceType
                            {
                                ServiceId = reader.GetInt32(serviceIdOrd),
                                ServicesName = reader.GetString(servicesNameOrd),
                            });
                        }
                    }

                }

            }

            return services;
        }

    }
}