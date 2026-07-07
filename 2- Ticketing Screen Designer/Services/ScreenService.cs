using System.Collections.Generic;
using Ticketing_Screen_Designer.DTO.Screens;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Utils;
namespace Ticketing_Screen_Designer.Services
{
    public class ScreenService : IScreenService
    {
        private readonly IFetchableRepository<ScreenModel> _fetchRepository;
        private readonly IAddableRepository<ScreenModel> _addRepository;
        private readonly IUpdateableRepository<ScreenModel> _updateRepository;
        private readonly IDeleteableRepository<ScreenModel> _deleteRepository;
        private readonly IListableRepository<ScreenModel> _listRepository;
        public ScreenService(IFetchableRepository<ScreenModel> fetchRepository,
            IAddableRepository<ScreenModel> addRepository,
            IUpdateableRepository<ScreenModel> updateRepository,
            IDeleteableRepository<ScreenModel> deleteRepository,
            IListableRepository<ScreenModel> listRepository)
        {
            _fetchRepository = fetchRepository;
            _addRepository = addRepository;
            _updateRepository = updateRepository;
            _deleteRepository = deleteRepository;
            _listRepository = listRepository;
        }
        public ScreenResponseDto GetScreenDetails(int screenId)
        {
            var screen = _fetchRepository.GetById(screenId);
            if (screen == null)
            {
                return null;
            }
            return new ScreenResponseDto
            {
                ScreenId = screen.ScreenId,
                BankId = screen.BankId,
                ScreenName = screen.ScreenName,
                IsActive = screen.IsActive,
                ModifiedAt = screen.ModifiedAt
            };

        }
        public List<ScreenResponseDto> GetAllScreensDetails(int bankId)
        {
            List<ScreenResponseDto> screensPerBank = new List<ScreenResponseDto>();
            var screens = _listRepository.GetAll(bankId);
            foreach (var screen in screens)
            {
                screensPerBank.Add(new ScreenResponseDto
                {
                    ScreenId = screen.ScreenId,
                    BankId = screen.BankId,
                    ScreenName = screen.ScreenName,
                    IsActive = screen.IsActive,
                    ModifiedAt = screen.ModifiedAt
                });
            }
            return screensPerBank;
        }
        public int AddScreen(CreateScreenRequestDto request)
        {
            ValidationExtensions.ValidateModel(request);
            var screenModel = new ScreenModel
            {
                ScreenName = request.ScreenName,
                BankId = request.BankId
            };
            int generatedId = _addRepository.Add(screenModel);
            return generatedId;
        }
        public bool UpdateScreen(BaseScreenRequestDto request)
        {
            ValidationExtensions.ValidateModel(request);
            var screenModel = new ScreenModel
            {
                ScreenId = request.screenId,
                ScreenName = request.ScreenName,
                IsActive = request.IsActive
            };
            bool isUpdated = _updateRepository.Update(screenModel);
            return isUpdated;
        }
        public bool DeleteScreen(int screenId)
        {
            bool isDeleted = _deleteRepository.Delete(screenId);
            return isDeleted;
        }
    }
}

