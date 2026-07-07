using System.Collections.Generic;
using Ticketing_Screen_Designer.DTO.ButtonTypes;

namespace Ticketing_Screen_Designer.Interfaces.Services
{
    public interface IButtonTypeService
    {
        ButtonTypeResponseDto GetButtonType(int id);
        List<ButtonTypeResponseDto> GetAllButtonTypes();
    }
}
