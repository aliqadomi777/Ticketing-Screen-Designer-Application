using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
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
            catch (SqlException ex)
            {
                Log.Error(ex,
                          "SQL error {SqlErrorNumber} while retrieving screen with ID: '{screenId}'.",
                          ex.Number,
                          screenId);

                throw new DataAccessException(
                    "A database error occurred while retrieving screen info",
                    ex);
            }


            catch (Exception ex)
            {
                Log.Error(ex,
                    "Failed business operation 'GetScreenDetails' for screen {screenId}.",
                    screenId);

                throw new DataAccessException(
                    $"Could not retrieve screen {screenId}.",
                    ex);
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
            catch (SqlException ex)
            {
                Log.Error(ex,
                          "SQL error {SqlErrorNumber} while retrieving screens for bank with ID: '{bankId}'.",
                          ex.Number,
                          bankId);

                throw new DataAccessException(
                    "A database error occurred while retrieving all screens for the bank",
                    ex);
            }


            catch (Exception ex)
            {
                Log.Error(ex,
                    "Failed business operation 'GetAllScreensDetails' for bank {bankId}.",
                    bankId);

                throw new DataAccessException(
                    $"Could not retrieve screens with bank ID: {bankId}.",
                    ex);
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

            catch (DuplicateRecordException)
            {
                throw;
            }

            catch (ParentDeletedWithChildConflictException)
            {
                throw;
            }

            catch (SqlException ex)
            {
                Log.Error(ex,
                          "SQL error {SqlErrorNumber} while creating Screen '{ScreenName}'.",
                          ex.Number,
                          request.ScreenName);

                throw new DataAccessException(
                    "A database error occurred while creating the screen.",
                    ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                    "Unexpected error while creating screen '{ScreenName}'.",
                    request.ScreenName);

                throw new DataAccessException(
                    "An unexpected error occurred while creating the Screen.",
                    ex);
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
            catch (DuplicateRecordException)
            {
                throw;
            }
            catch (ExcessiveScreenActivationException)
            {
                throw;
            }
            catch (SqlException ex)
            {
                Log.Error(ex,
                          "SQL error {SqlErrorNumber} while updating Screen '{ScreenName}'.",
                          ex.Number,
                          request.ScreenName);

                throw new DataAccessException(
                    "A database error occurred while updating the screen.",
                    ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                    "Unexpected error while updating screen '{ScreenName}'.",
                    request.ScreenName);

                throw new DataAccessException(
                    "An unexpected error occurred while updating the ScreenName.",
                    ex);
            }

        }
        public bool DeleteScreen(int screenId)
        {
            try
            {
                bool isDeleted = _deleteRepository.Delete(screenId);
                return isDeleted;
            }
            catch (SqlException ex)
            {
                Log.Error(ex,
                          "SQL error {SqlErrorNumber} while deleting Screen with ID: '{screenId}'.",
                          ex.Number,
                          screenId);

                throw new DataAccessException(
                    "A database error occurred while deleting the screen.",
                    ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                    "Unexpected error while deleting screen with ID: '{screenId}'.",
                    screenId);

                throw new DataAccessException(
                    "An unexpected error occurred while deleting the Screen.",
                    ex);
            }

        }
    }
}

