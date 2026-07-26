using App.Application.DTO.ButtonTypes;
using System.Collections.Generic;
namespace App.Application.Interfaces
{
    public interface IButtonTypeService
    {
        ButtonTypeResponseDto GetButtonType(int id);
        List<ButtonTypeResponseDto> GetAllButtonTypes();
    }
}
