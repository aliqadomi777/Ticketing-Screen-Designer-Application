using System.Collections.Generic;
using Ticketing_Screen_Designer.DTO.Buttons;
using Ticketing_Screen_Designer.DTO.Screens;
namespace Ticketing_Screen_Designer.Interfaces.Services
{
    public interface IScreenService
    {
        ScreenResponseDto GetScreenDetails(int id);

        //Retrieve All screen for A bank -> ID
        List<ScreenResponseDto> GetAllScreensDetails(int id);
        int AddScreen(CreateScreenRequestDto screenRequest);
        bool UpdateScreen(BaseScreenRequestDto request);
        bool DeleteScreen(int id);
    }
}
