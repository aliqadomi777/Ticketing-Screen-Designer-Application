using App.Application.DTO.ServiceTypes;
using System.Collections.Generic;

namespace App.Application.Interfaces
{
    public interface IServiceTypeService
    {
        ServiceTypeResponseDto GetServiceType(int id);
        List<ServiceTypeResponseDto> GetAllServices();
    }
}
