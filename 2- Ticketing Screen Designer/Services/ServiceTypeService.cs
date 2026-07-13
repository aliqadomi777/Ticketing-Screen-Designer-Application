using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using Ticketing_Screen_Designer.DTO.Services;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Utils;
namespace Ticketing_Screen_Designer.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IFetchableRepository<ServiceType> _fetchRepository;
        private readonly IGetAllRepository<ServiceType> _fetchAllRepository;
        public ServiceTypeService(IFetchableRepository<ServiceType> fetchRepository, IGetAllRepository<ServiceType> fetchAllRepository)
        {
            _fetchRepository = fetchRepository;
            _fetchAllRepository = fetchAllRepository;
        }

        public ServiceTypeResponseDto GetServiceType(int serviceId)
        {
            if (serviceId <= 0)
            {
                throw new ArgumentException("service ID must be a positive non-zero integer.", nameof(serviceId));
            }
            try
            {
                var serviceType = _fetchRepository.GetById(serviceId);
                return new ServiceTypeResponseDto
                {
                    ServiceId = serviceType.ServiceId,
                    ServicesName = serviceType.ServicesName
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed executing query retrieving service type with ID: {serviceId}", serviceId);
                throw new DataAccessException($"Could not retrieve profile service type for service ID {serviceId}.", ex);
            }
        }

        public List<ServiceTypeResponseDto> GetAllServices()
        {
            try
            {
                var serviceTypes = _fetchAllRepository.GetAll();
                return serviceTypes.Select(serviceType => new ServiceTypeResponseDto
                {
                    ServiceId = serviceType.ServiceId,
                    ServicesName = serviceType.ServicesName
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed executing query retrieving all service types");
                throw new DataAccessException("Could not retrieve all service types", ex);
            }

        }
    }
}
