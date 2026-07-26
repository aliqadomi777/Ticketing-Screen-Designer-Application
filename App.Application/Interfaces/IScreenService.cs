using App.Application.DTO.Buttons;
using App.Application.DTO.Screens;
using System.Collections.Generic;
namespace App.Application.Interfaces
{
    public interface IScreenService
    {
        ScreenResponseDto GetScreenDetails(int id);

        //Retrieve All screen for A bank -> ID
        List<ScreenResponseDto> GetAllScreensDetails(int id);
        int AddScreen(CreateScreenRequestDto request);
        bool UpdateScreen(BaseScreenRequestDto request);
        bool DeleteScreen(int id);

        int CreateScreenWithButtons(CreateScreenRequestDto request,
            IEnumerable<BaseButtonDto> requests);
        bool UpdateScreenAndButtons(BaseScreenRequestDto screenUpdate,
                            IEnumerable<BaseButtonDto> pendingCreates,
                            IEnumerable<UpdateButtonRequestDto> pendingUpdates,
                            IEnumerable<int> pendingDeletes);
    }
}
