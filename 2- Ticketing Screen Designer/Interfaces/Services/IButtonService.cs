using System.Collections.Generic;
using Ticketing_Screen_Designer.DTO.Buttons;


namespace Ticketing_Screen_Designer.Interfaces.Services
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
