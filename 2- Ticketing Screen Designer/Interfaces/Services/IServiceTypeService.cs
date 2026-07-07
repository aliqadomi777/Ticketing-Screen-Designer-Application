using System.Collections.Generic;
using Ticketing_Screen_Designer.DTO.Services;

namespace Ticketing_Screen_Designer.Interfaces.Services
{
    public interface IServiceTypeService
    {
        ServiceTypeResponseDto GetButtonType(int id);
        List<ServiceTypeResponseDto> GetAllButtonTypes();
    }
}
