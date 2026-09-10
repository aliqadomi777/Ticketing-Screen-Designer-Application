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
        private readonly IFetchableRepository<ServiceModel> _fetchRepository;
        private readonly IListableRepository<ServiceModel> _fetchAllRepository;
        private readonly ILogger<ServiceTypeService> _logger;

        public ServiceTypeService(IFetchableRepository<ServiceModel> fetchRepository, IListableRepository<ServiceModel> fetchAllRepository,
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
                    ServiceNameEN = serviceType.ServiceNameEN,
                    ServiceNameAR = serviceType.ServiceNameAR,
                    MinimumServiceTime = serviceType.MinimumServiceTime,
                    MaximumServiceTime = serviceType.MaximumServiceTime,
                    MaxTicketsPerDay = serviceType.MaxTicketsPerDay,
                    IsActive = serviceType.IsActive,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, serviceId);
                throw;
            }
        }

        public List<ServiceTypeResponseDto> GetAllServices(int bankId)
        {
            try
            {
                var serviceTypes = _fetchAllRepository.GetAll(bankId);
                return serviceTypes.Select(serviceType => new ServiceTypeResponseDto
                {
                    ServiceId = serviceType.ServiceId,
                    ServiceNameEN = serviceType.ServiceNameEN,
                    ServiceNameAR = serviceType.ServiceNameAR,
                    MinimumServiceTime = serviceType.MinimumServiceTime,
                    MaximumServiceTime = serviceType.MaximumServiceTime,
                    MaxTicketsPerDay = serviceType.MaxTicketsPerDay,
                    IsActive = serviceType.IsActive,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }
    }
}
