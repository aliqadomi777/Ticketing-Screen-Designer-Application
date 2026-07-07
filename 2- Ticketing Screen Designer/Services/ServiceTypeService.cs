using System.Collections.Generic;
using Ticketing_Screen_Designer.DTO.Services;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Models;
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

        public ServiceTypeResponseDto GetButtonType(int typeId)
        {
            var serviceType = _fetchRepository.GetById(typeId);
            return new ServiceTypeResponseDto
            {
                ServiceId = serviceType.ServiceId,
                ServicesName = serviceType.ServicesName
            };
        }

        public List<ServiceTypeResponseDto> GetAllButtonTypes()
        {
            List<ServiceTypeResponseDto> serviceTypesList = new List<ServiceTypeResponseDto>();
            var serviceTypes = _fetchAllRepository.GetAll();
            foreach (var serviceType in serviceTypes)
            {
                serviceTypesList.Add(new ServiceTypeResponseDto
                {
                    ServiceId = serviceType.ServiceId,
                    ServicesName = serviceType.ServicesName
                });
            }
            return serviceTypesList;
        }
    }
}
