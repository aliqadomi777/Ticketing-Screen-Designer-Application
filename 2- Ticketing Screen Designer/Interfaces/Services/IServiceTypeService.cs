using System.Collections.Generic;
using Ticketing_Screen_Designer.DTO.Services;

namespace Ticketing_Screen_Designer.Interfaces.Services
{
    public interface IServiceTypeService
    {
        ServiceTypeResponseDto GetServiceType(int id);
        List<ServiceTypeResponseDto> GetAllServices();
    }
}
