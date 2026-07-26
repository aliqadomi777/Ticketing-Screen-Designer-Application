using App.Application.DTO.Buttons;

using System.Collections.Generic;

namespace App.Application.Interfaces
{
    public interface IButtonService
    {

        BaseButtonResponseDto GetButtonDetails(int id, int type);
        List<BaseButtonResponseDto> GetAllButtonsDetails(int id);
        bool UpdateButtons(IEnumerable<UpdateButtonRequestDto> requests);
        bool DeleteButtons(IEnumerable<int> ids);
    }
    public interface IAddButtonService
    {
        List<int> AddButtons(IEnumerable<BaseButtonDto> requests);
    }


}
