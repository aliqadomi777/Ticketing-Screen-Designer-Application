using App.Domain.Interfaces;
using App.Domain.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace App.Infrastructure.Repositories
{
    public class ServiceRepository : BaseRepository,
        IFetchableRepository<ServiceModel>,
        IListableRepository<ServiceModel>
    {
        public ServiceRepository(string connectionString) : base(connectionString) { }
        public ServiceModel GetById(int serviceId)
        {
            string query = @"
                SELECT ServiceID, ServiceNameEN 
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
                        return new ServiceModel
                        {
                            ServiceId = reader.GetInt32(reader.GetOrdinal("ServiceID")),
                            ServiceNameEN = reader.GetString(reader.GetOrdinal("ServiceNameEN")),

                        };
                    }
                }
            }

            return null;


        }
        public IEnumerable<ServiceModel> GetAll(int bankId)
        {
            string query = @"
                SELECT ServiceID, ServiceNameEN
                FROM Services
                WHERE BankID=@BankID;";
            List<ServiceModel> services = new List<ServiceModel>();

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = bankId;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        int serviceIdOrd = reader.GetOrdinal("ServiceID");
                        int serviceNameENOrd = reader.GetOrdinal("ServiceNameEN");

                        while (reader.Read())
                        {
                            services.Add(new ServiceModel
                            {
                                ServiceId = reader.GetInt32(serviceIdOrd),
                                ServiceNameEN = reader.GetString(serviceNameENOrd),
                            });
                        }
                    }

                }

            }

            return services;
        }

    }
}