using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ScreenService(
            IFetchableRepository<ScreenModel> fetchRepository,
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
            if (screenId <= 0)
            {
                throw new ArgumentException("Screen ID must be a positive non-zero integer.", nameof(screenId));
            }
            try
            {
                var screen = _fetchRepository.GetById(screenId);
                return screen == null ? null : new ScreenResponseDto
                {
                    ScreenId = screen.ScreenId,
                    BankId = screen.BankId,
                    ScreenName = screen.ScreenName,
                    IsActive = screen.IsActive,
                    ModifiedAt = screen.ModifiedAt
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed executing query for Screen with ID: {screenId}", screenId);
                throw new DataAccessException($"Could not retrieve profile records for Screen ID {screenId}.", ex);
            }

        }
        public List<ScreenResponseDto> GetAllScreensDetails(int bankId)
        {
            if (bankId <= 0)
            {
                throw new ArgumentException("Bank ID must be a positive non-zero integer.", nameof(bankId));
            }
            try
            {
                var screens = _listRepository.GetAll(bankId);
                return screens.Select(screen => new ScreenResponseDto
                {
                    ScreenId = screen.ScreenId,
                    BankId = screen.BankId,
                    ScreenName = screen.ScreenName,
                    IsActive = screen.IsActive,
                    ModifiedAt = screen.ModifiedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed executing query retrieving all screen for bank with ID: {bankId}", bankId);
                throw new DataAccessException($"Could not retrieve profile screens for bank ID {bankId}.", ex);
            }
        }
        public int AddScreen(CreateScreenRequestDto request)
        {
            ValidationExtensions.ValidateModel(request);
            try
            {
                var screenModel = new ScreenModel
                {
                    ScreenName = request.ScreenName,
                    BankId = request.BankId,
                    IsActive = request.IsActive,

                };
                int generatedId = _addRepository.Add(screenModel);
                return generatedId;
            }

            catch (Exception ex) when (!(ex is DuplicateRecordException))
            {
                Log.Error(ex, "Unexpected failure during Screen creation process.");
                throw new DataAccessException("An unexpected structural error occurred. Please try again later.", ex);
            }
        }
        public bool UpdateScreen(BaseScreenRequestDto request)
        {
            ValidationExtensions.ValidateModel(request);
            try
            {
                var screenModel = new ScreenModel
                {
                    ScreenId = request.screenId,
                    ScreenName = request.ScreenName,
                    IsActive = request.IsActive
                };
                bool isUpdated = _updateRepository.Update(screenModel);
                return isUpdated;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected failure during Screen Updating process.");
                throw new DataAccessException("An unexpected structural error occurred. Please try again later.", ex);
            }

        }
        public bool DeleteScreen(int screenId)
        {
            try
            {
                bool isDeleted = _deleteRepository.Delete(screenId);
                return isDeleted;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected failure during Screen Deletion process.");
                throw new DataAccessException("An unexpected structural error occurred. Please try again later.", ex);
            }

        }
    }
}

