using App.Application.DTO.Buttons;
using App.Application.Interfaces;
using App.Domain.Interfaces;
using App.Domain.Models;
using App.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
namespace App.Application.Services
{
    public class ButtonService : IButtonService, IAddButtonService
    {
        private readonly IButtonRepository<ButtonModel> _fetchButtonRepository;
        private readonly IUpdateableRepository<ButtonModel> _updateButtonRepository;
        private readonly IDeleteableRepository<ButtonModel> _deleteButtonRepository;
        private readonly IListableRepository<ButtonModel> _listButtonRepository;
        private readonly IAddableRepository<ButtonModel> _addButtonRepository;

        private readonly IDeleteableRepository<TicketModel> _deleteTicketRepository;
        private readonly IAddableRepository<TicketModel> _addTicketRepository;
        private readonly ITicketRepository<TicketModel> _updateTicketRepository;

        private readonly IDeleteableRepository<MessageModel> _deleteMessageRepository;
        private readonly IAddableRepository<MessageModel> _addMessageRepository;
        private readonly IUpdateableRepository<MessageModel> _updateMessageRepository;

        private readonly ILogger<ButtonService> _logger;

        public ButtonService(
            IButtonRepository<ButtonModel> fetchButtonRepository,
            IUpdateableRepository<ButtonModel> updateButtonRepository,
            IDeleteableRepository<ButtonModel> deleteButtonRepository,
            IListableRepository<ButtonModel> listButtonRepository,
            IDeleteableRepository<TicketModel> deleteTicketRepository,
            IAddableRepository<TicketModel> addTicketRepository,
            IDeleteableRepository<MessageModel> deleteMessageRepository,
            IAddableRepository<MessageModel> addMessageRepository,
            IAddableRepository<ButtonModel> addButtonRepository,
            ITicketRepository<TicketModel> updateTicketRepository,
            IUpdateableRepository<MessageModel> updateMessageRepository,
            ILogger<ButtonService> logger
            )
        {
            _fetchButtonRepository = fetchButtonRepository;
            _updateButtonRepository = updateButtonRepository;
            _deleteButtonRepository = deleteButtonRepository;
            _listButtonRepository = listButtonRepository;
            _deleteTicketRepository = deleteTicketRepository;
            _addTicketRepository = addTicketRepository;
            _deleteMessageRepository = deleteMessageRepository;
            _addMessageRepository = addMessageRepository;
            _addButtonRepository = addButtonRepository;
            _updateMessageRepository = updateMessageRepository;
            _updateTicketRepository = updateTicketRepository;
            _logger = logger;
        }

        public BaseButtonResponseDto GetButtonDetails(int buttonId, int buttonType)
        {
            if (buttonId <= 0)
            {
                throw new ArgumentException("button ID must be a positive non-zero integer.", nameof(buttonId));
            }
            if (buttonType <= 0)
            {
                throw new ArgumentException("button type must be a positive non-zero integer.", nameof(buttonType));
            }


            try
            {
                var button = _fetchButtonRepository.GetById(buttonId, buttonType);
                if (button == null)
                {
                    return null;
                }
                if (button is TicketModel)
                {
                    TicketModel ticketModel = (TicketModel)button;
                    return new TicketButtonResponseDto
                    {
                        ButtonId = ticketModel.ButtonId,
                        ButtonNameAR = ticketModel.ButtonNameAR,
                        ButtonNameEN = ticketModel.ButtonNameEN,
                        ButtonType = ticketModel.ButtonType,
                        ScreenId = ticketModel.ScreenId,
                        ModifiedAt = ticketModel.ModifiedAt,
                        ServiceId = ticketModel.ServiceId,
                        ServiceName = ticketModel.ServiceName,
                        TicketId = ticketModel.TicketId,
                        TypeName = ticketModel.TypeName

                    };
                }
                else if (button is MessageModel)
                {
                    MessageModel messageModel = (MessageModel)button;
                    return new MessageButtonResponseDto
                    {
                        ButtonId = messageModel.ButtonId,
                        ButtonNameAR = messageModel.ButtonNameAR,
                        ButtonNameEN = messageModel.ButtonNameEN,
                        ButtonType = messageModel.ButtonType,
                        ScreenId = messageModel.ScreenId,
                        ModifiedAt = messageModel.ModifiedAt,
                        TypeName = messageModel.TypeName,
                        MessageAR = messageModel.MessageAR,
                        MessageEN = messageModel.MessageEN,
                        MessageId = messageModel.MessageId
                    };
                }
                return null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex,
                          ex.Message,
                          ex.Number,
                          buttonId);

                throw;

            }


            catch (Exception ex)
            {
                _logger.LogError(ex,
                    ex.Message,
                    buttonId);

                throw;

            }
        }
        public List<BaseButtonResponseDto> GetAllButtonsDetails(int screenId)
        {
            try
            {
                var buttons = _listButtonRepository.GetAll(screenId);
                return buttons.Select(button => new BaseButtonResponseDto
                {
                    ButtonId = button.ButtonId,
                    ScreenId = button.ScreenId,
                    ButtonNameAR = button.ButtonNameAR,
                    ButtonNameEN = button.ButtonNameEN,
                    ButtonType = button.ButtonType,
                    ModifiedAt = button.ModifiedAt,
                    TypeName = button.TypeName
                }).ToList();
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
        /* 
          adding button is wrapped by transaction to ensure both operations on 
          button table and ticket or message table are commited both or rolled back 
          First base button is added to button table then child row into tickets or messages
          
         */


        public List<int> AddButtons(IEnumerable<BaseButtonDto> requests)
        {
            var resultIds = new List<int>();

            using (var scope = new System.Transactions.TransactionScope(
                    System.Transactions.TransactionScopeOption.Required,
                     new System.Transactions.TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            {
                try
                {

                    foreach (var request in requests)
                    {
                        int newButtonId = 0;
                        int resultId = 0;
                        var button = new BaseButtonRequestDto
                        {
                            ButtonNameEN = request.ButtonNameEN,
                            ButtonNameAR = request.ButtonNameAR,
                            ButtonType = request.ButtonType,
                            ScreenId = request.ScreenId
                        };
                        newButtonId = _addButtonRepository.Add(new ButtonModel
                        {
                            ButtonNameAR = button.ButtonNameAR,
                            ButtonType = button.ButtonType,
                            ButtonNameEN = button.ButtonNameEN,
                            ScreenId = button.ScreenId
                        });


                        if (request is CreateTicketButtonRequestDto newTicketButton)
                        {

                            resultId = _addTicketRepository.Add(new TicketModel
                            {
                                ButtonId = newButtonId,
                                ServiceId = newTicketButton.ServiceId
                            });

                        }
                        else if (request is CreateMessageButtonRequestDto newMessageButton)
                        {
                            resultId = _addMessageRepository.Add(new MessageModel
                            {
                                ButtonId = newButtonId,
                                MessageEN = newMessageButton.MessageEN,
                                MessageAR = newMessageButton.MessageAR,
                            });

                        }
                        else
                        {
                            throw new NotSupportedException("Unsupported button type: " + request.ButtonType);
                        }
                        resultIds.Add(resultId);

                    }
                    scope.Complete();
                    return resultIds;

                }
                catch (Exception ex) when (ex is NotSupportedException || ex is ParentDeletedWithChildConflictException ||
                                           ex is DuplicateRecordException || ex is ValidationException)
                {
                    throw;
                }
                catch (SqlException ex)
                {
                    _logger.LogError(ex, ex.Message, ex.Number);
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }
            }
        }

        /* 
            updating button is wrapped by transaction to ensure both operations on button 
            table and ticket or message table are commited on both or rolled back
            
            cases : 
            - updating a button that was deleted : return when fetched null -> try to be updated -> does not exist -> !isButtonUpdated 
            - updating a button but changing type : returns when fetched null (in Ui Info is passed as the new info to be updated) :
              queries are inner joins : new buttontype  != original buttontype -> always isButtonUpdated = true -> the deletion must take place
            -> true if issue happened it will be false and it rollback automatically -> scope completes return true 
            - updating a button while retaining the same button type : return when fetched button -> either base button updated 
            or the child row ticket or message or both -> returns true
         */
        public bool UpdateButtons(IEnumerable<UpdateButtonRequestDto> requests)
        {


            using (var scope = new System.Transactions.TransactionScope(
        System.Transactions.TransactionScopeOption.Required,
         new System.Transactions.TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            {
                try
                {
                    foreach (var request in requests)
                    {
                        var currentButton = _fetchButtonRepository.GetById(request.ButtonId, request.ButtonType);
                        //Either button deleted or it's child type is 
                        if (currentButton == null)
                        {
                            var buttonModel = new ButtonModel
                            {
                                ButtonNameAR = request.ButtonNameAR,
                                ButtonNameEN = request.ButtonNameEN,
                                ButtonType = request.ButtonType,
                                ButtonId = request.ButtonId,
                            };
                            // if false : does not exist 
                            bool isButtonUpdated = _updateButtonRepository.Update(buttonModel);
                            if (request.ButtonType == 1 && isButtonUpdated)
                            {
                                var ticket = (UpdateTicketButtonRequest)request;
                                var ticketModel = new TicketModel
                                {
                                    ButtonId = ticket.ButtonId,
                                    ScreenId = ticket.ScreenId,
                                    ServiceId = ticket.ServiceId,
                                    ButtonNameEN = ticket.ButtonNameEN,
                                    ButtonNameAR = ticket.ButtonNameAR,
                                    ButtonType = ticket.ButtonType
                                };
                                bool isDeleted = _deleteMessageRepository.Delete(request.ButtonId);
                                _addTicketRepository.Add(ticketModel);

                            }
                            else if (request.ButtonType == 2 && isButtonUpdated)
                            {
                                var message = (UpdateMessageButtonRequest)request;

                                var messageModel = new MessageModel
                                {
                                    ButtonId = message.ButtonId,
                                    ScreenId = message.ScreenId,
                                    ButtonNameAR = message.ButtonNameAR,
                                    ButtonNameEN = message.ButtonNameEN,
                                    MessageAR = message.MessageAR,
                                    MessageEN = message.MessageEN,
                                    ButtonType = message.ButtonType
                                };
                                bool isDeleted = _deleteTicketRepository.Delete(request.ButtonId);
                                _addMessageRepository.Add(messageModel);

                            }

                            //else
                            //{
                            //    throw new NotSupportedException("Unsupported button type: " + request.ButtonType);
                            //}


                        }

                        //Editing the same button retaining the same type
                        else if (request.ButtonType == currentButton.ButtonType)
                        {
                            var buttonModel = new ButtonModel
                            {
                                ButtonNameAR = request.ButtonNameAR,
                                ButtonNameEN = request.ButtonNameEN,
                                ButtonType = request.ButtonType,
                                ButtonId = request.ButtonId,
                            };
                            bool isButtonUpdated = _updateButtonRepository.Update(buttonModel);

                            if (request is UpdateTicketButtonRequest updatedTicket)
                            {
                                bool isTicketUpdated = _updateTicketRepository.Update(updatedTicket.ServiceId, updatedTicket.TicketId);
                            }
                            else if (request is UpdateMessageButtonRequest updatedMessage)
                            {
                                bool isMessageUpdated = _updateMessageRepository.Update(new MessageModel
                                {
                                    MessageId = updatedMessage.messageId,
                                    MessageAR = updatedMessage.MessageAR,
                                    MessageEN = updatedMessage.MessageEN,
                                });
                            }


                        }


                    }
                    scope.Complete();
                    return true;
                }

                catch (Exception ex) when (ex is NotSupportedException || ex is DuplicateRecordException)
                {
                    throw;
                }
                catch (SqlException ex)
                {
                    _logger.LogError(ex, ex.Message, ex.Number);
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }
            }
        }
        public bool DeleteButtons(IEnumerable<int> buttonIds)
        {
            using (var scope = new System.Transactions.TransactionScope(
                System.Transactions.TransactionScopeOption.Required,
                new System.Transactions.TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            {
                try
                {
                    foreach (var buttonId in buttonIds)
                    {
                        bool isDeleted = _deleteButtonRepository.Delete(buttonId);
                    }

                    scope.Complete();
                    return true;
                }
                catch (KeyNotFoundException)
                {
                    throw;
                }
                catch (SqlException ex)
                {
                    _logger.LogError(ex, ex.Message, ex.Number);
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }
            }
        }


    }
}
