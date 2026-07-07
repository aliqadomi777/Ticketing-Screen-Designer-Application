using System.Collections.Generic;
using Ticketing_Screen_Designer.DTO.Buttons;


namespace Ticketing_Screen_Designer.Interfaces.Services
{
    public interface IButtonService
    {

        BaseButtonResponseDto GetButtonDetails(int id, int type);
        List<BaseButtonResponseDto> GetAllButtonsDetails(int id);
        int AddButton(BaseButtonDto request);
        bool UpdateButton(UpdateButtonRequestDto request);
        bool DeleteButton(int id);
    }
}
