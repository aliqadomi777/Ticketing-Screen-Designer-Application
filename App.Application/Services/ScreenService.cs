using App.Application.DTO.Buttons;
using App.Application.DTO.Screens;
using App.Application.Interfaces;
using App.Domain.Interfaces;
using App.Domain.Models;
using App.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
namespace App.Application.Services
{
    public class ScreenService : IScreenService
    {
        private readonly IFetchableRepository<ScreenModel> _fetchRepository;
        private readonly IAddableRepository<ScreenModel> _addRepository;
        private readonly IUpdateableRepository<ScreenModel> _updateRepository;
        private readonly IDeleteableRepository<ScreenModel> _deleteRepository;
        private readonly IListableRepository<ScreenModel> _listRepository;


        private readonly IAddButtonService _addButtonService;
        private readonly IButtonService _buttonService;

        private readonly ILogger<ScreenService> _logger;

        public ScreenService(
            IFetchableRepository<ScreenModel> fetchRepository,
            IAddableRepository<ScreenModel> addRepository,
            IUpdateableRepository<ScreenModel> updateRepository,
            IDeleteableRepository<ScreenModel> deleteRepository,
            IListableRepository<ScreenModel> listRepository,
            IAddButtonService addButtonService,
            IButtonService buttonService,
            ILogger<ScreenService> logger)
        {
            _fetchRepository = fetchRepository;
            _addRepository = addRepository;
            _updateRepository = updateRepository;
            _deleteRepository = deleteRepository;
            _listRepository = listRepository;
            _addButtonService = addButtonService;
            _buttonService = buttonService;
            _logger = logger;
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
                _logger.LogError(ex,
                          ex.Message,
                          ex.Number,
                          screenId);
                throw;

            }


            catch (Exception ex)
            {
                _logger.LogError(ex,
                    ex.Message,
                    screenId);

                throw;

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
                _logger.LogError(ex,
                          ex.Message,
                          ex.Number,
                          bankId);
                throw;

            }


            catch (Exception ex)
            {
                _logger.LogError(ex,
                    ex.Message,
                    bankId);
                throw;

            }
        }
        public int AddScreen(CreateScreenRequestDto screenRequest)
        {
            try
            {
                ValidationExtensions.ValidateModel(screenRequest);
                var screenModel = new ScreenModel
                {
                    ScreenName = screenRequest.ScreenName,
                    BankId = screenRequest.BankId,
                    IsActive = screenRequest.IsActive,

                };
                int newScreenId = _addRepository.Add(screenModel);
                return newScreenId;
            }

            catch (DuplicateRecordException)
            {
                throw;
            }

            catch (ParentDeletedWithChildConflictException)
            {
                throw;
            }
            catch (ExcessiveScreenActivationException)
            {
                throw;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex,
                          ex.Message,
                          ex.Number,
                          screenRequest.ScreenName);

                throw;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    ex.Message,
                    screenRequest.ScreenName);

                throw;

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
                _logger.LogError(ex,
                          ex.Message,
                          ex.Number,
                          request.ScreenName);

                throw;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    ex.Message,
                    request.ScreenName);

                throw;

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
                _logger.LogError(ex,
                          ex.Message,
                          ex.Number,
                          screenId);

                throw;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    ex.Message,
                    screenId);

                throw;

            }

        }
        public int CreateScreenWithButtons(CreateScreenRequestDto screenRequest,
                                           IEnumerable<BaseButtonDto> buttonRequests)
        {
            using (var scope = new System.Transactions.TransactionScope(
                System.Transactions.TransactionScopeOption.Required,
                new System.Transactions.TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            {
                try
                {
                    int newScreenId = _addRepository.Add(new ScreenModel
                    {
                        ScreenName = screenRequest.ScreenName,
                        BankId = screenRequest.BankId,
                        IsActive = screenRequest.IsActive
                    });

                    foreach (var button in buttonRequests)
                    {
                        button.ScreenId = newScreenId;
                    }
                    _addButtonService.AddButtons(buttonRequests);

                    scope.Complete();
                    return newScreenId;
                }
                catch (DuplicateRecordException)
                {
                    throw;
                }

                catch (ParentDeletedWithChildConflictException)
                {
                    throw;
                }
                catch (ExcessiveScreenActivationException)
                {
                    throw;
                }
                catch (SqlException ex)
                {
                    _logger.LogError(ex,
                              ex.Message,
                              ex.Number,
                              screenRequest.ScreenName);
                    throw;

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        ex.Message,
                        screenRequest.ScreenName);

                    throw;

                }

            }
        }



        public bool UpdateScreenAndButtons(
            BaseScreenRequestDto screenUpdate,
            IEnumerable<BaseButtonDto> pendingCreates,
            IEnumerable<UpdateButtonRequestDto> pendingUpdates,
            IEnumerable<int> pendingDeletes)
        {
            using (var scope = new System.Transactions.TransactionScope(
                System.Transactions.TransactionScopeOption.Required,
                new System.Transactions.TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            {
                try
                {
                    if (screenUpdate != null)
                    {
                        _updateRepository.Update(new ScreenModel
                        {
                            ScreenId = screenUpdate.screenId,
                            ScreenName = screenUpdate.ScreenName,
                            IsActive = screenUpdate.IsActive
                        });
                    }
                    //This order -> solved name conflicts -> front end depends on current logic
                    if (pendingDeletes.Any())
                    {
                        _buttonService.DeleteButtons(pendingDeletes);
                    }
                    if (pendingUpdates.Any())
                    {
                        _buttonService.UpdateButtons(pendingUpdates);
                    }
                    if (pendingCreates.Any())
                    {
                        _addButtonService.AddButtons(pendingCreates);

                    }


                    scope.Complete();
                    return true;
                }
                catch (ParentDeletedWithChildConflictException)
                {
                    throw;
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
                    _logger.LogError(ex,
                              ex.Message,
                              ex.Number,
                              screenUpdate.ScreenName);

                    throw;

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        ex.Message,
                        screenUpdate.ScreenName);
                    throw;

                }
            }
        }
    }
}

