using App.Application.DTO.ServiceTypes;
using App.Application.Interfaces;
using App.Domain.Interfaces;
using App.Domain.Models;
using App.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
namespace App.Application.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IFetchableRepository<ServiceType> _fetchRepository;
        private readonly IGetAllRepository<ServiceType> _fetchAllRepository;
        private readonly ILogger<ServiceTypeService> _logger;

        public ServiceTypeService(IFetchableRepository<ServiceType> fetchRepository, IGetAllRepository<ServiceType> fetchAllRepository,
            ILogger<ServiceTypeService> logger)
        {
            _fetchRepository = fetchRepository;
            _fetchAllRepository = fetchAllRepository;
            _logger = logger;
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
                _logger.LogError(ex, "Failed executing query retrieving service type with ID: {serviceId}", serviceId);
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
                _logger.LogError(ex, "Failed executing query retrieving all service types");
                throw new DataAccessException("Could not retrieve all service types", ex);
            }

        }
    }
}
