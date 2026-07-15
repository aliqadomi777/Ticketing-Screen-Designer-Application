using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Ticketing_Screen_Designer.DTO.Buttons;
using Ticketing_Screen_Designer.DTO.Tickets;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Utils;

namespace Ticketing_Screen_Designer.Services
{
    public class ButtonService : IButtonService
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
            IUpdateableRepository<MessageModel> updateMessageRepository

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
            catch (Exception ex)
            {
                Log.Error(ex, "Failed executing query for button with ID: {buttonId}", buttonId);
                throw new DataAccessException($"Could not retrieve profile records for button ID {buttonId}.", ex);
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
            catch (Exception ex)
            {
                Log.Error(ex, "Failed executing query for all buttons in screen with ID: {screenId}", screenId);
                throw new DataAccessException($"Could not retrieve profile records for screen ID {screenId}.", ex);
            }


        }
        //add validation + scope and transaction for multi instance
        public int AddButton(BaseButtonDto request)
        {
            try
            {
                int newButtonId = _addButtonRepository.Add(new ButtonModel
                {
                    ButtonNameAR = request.ButtonNameAR,
                    ButtonType = request.ButtonType,
                    ButtonNameEN = request.ButtonNameEN,
                    ScreenId = request.ScreenId
                });
                if (request is CreateTicketButtonRequestDto newTicketButton)
                {
                    int newTicketId = _addTicketRepository.Add(new TicketModel
                    {
                        ButtonId = newButtonId,
                        ServiceId = newTicketButton.ServiceId
                    });

                    return newTicketId;
                }
                else if (request is CreateMessageButtonRequestDto newMessageButton)
                {

                    int newMessageId = _addMessageRepository.Add(new MessageModel
                    {
                        ButtonId = newButtonId,
                        MessageEN = newMessageButton.MessageEN,
                        MessageAR = newMessageButton.MessageAR,
                    });
                    return newMessageId;
                }
                throw new NotSupportedException("Unsupported button type: " + request.ButtonType);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed adding a new button to screen: {screenId}", request.ScreenId);
                throw new DataAccessException($"Could add button records for screen ID {request.ScreenId}.", ex);
            }

        }
        public bool UpdateButton(UpdateButtonRequestDto request)
        {
            var currentButton = _fetchButtonRepository.GetById(request.ButtonId, request.ButtonType);

            if (currentButton == null)
            {
                var buttonModel = new ButtonModel
                {
                    ButtonNameAR = request.ButtonNameAR,
                    ButtonNameEN = request.ButtonNameEN,
                    ButtonType = request.ButtonType,
                    ButtonId = request.ButtonId,
                };
                bool isButtonUpdated = _updateButtonRepository.Update(buttonModel);
                if (request.ButtonType == 1)
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
                    if (isDeleted)
                    {

                        int generatedId = _addTicketRepository.Add(ticketModel);
                        return generatedId > 0;
                    }
                }
                else if (request.ButtonType == 2)
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
                    if (isDeleted)
                    {

                        int generatedId = _addMessageRepository.Add(messageModel);
                        return generatedId > 0;
                    }
                }


                throw new NotSupportedException("Unsupported button type: " + request.ButtonType);
            }
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
                    return isTicketUpdated && isButtonUpdated;
                }
                else if (request is UpdateMessageButtonRequest updatedMessage)
                {
                    bool isMessageUpdated = _updateMessageRepository.Update(new MessageModel
                    {
                        MessageId = updatedMessage.messageId,
                        MessageAR = updatedMessage.MessageAR,
                        MessageEN = updatedMessage.MessageEN,
                    });
                    return isMessageUpdated && isButtonUpdated;
                }
                throw new NotSupportedException("Unsupported button type: " + request.ButtonType);
            }
            throw new NotSupportedException("Unsupported button type: " + request.ButtonType);
            //else
            //{
            //    // changed from message into ticket
            //    if (request.ButtonType == 1)
            //    {
            //        var ticket = (UpdateTicketButtonRequest)request;
            //        var ticketModel = new TicketModel
            //        {
            //            ScreenId = ticket.ScreenId,
            //            ServiceId = ticket.ServiceId,
            //            ButtonNameEN = ticket.ButtonNameEN,
            //            ButtonNameAR = ticket.ButtonNameAR,
            //            ButtonType = ticket.ButtonType
            //        };
            //        bool isDeleted = _deleteMessageRepository.Delete(request.ButtonId);
            //        if (isDeleted)
            //        {

            //            int generatedId = _addTicketRepository.Add(ticketModel);
            //            return generatedId > 0;
            //        }
            //    }
            //    else if (request.ButtonType == 2)
            //    {
            //        var message = (UpdateMessageButtonRequest)request;
            //        var messageModel = new MessageModel
            //        {
            //            ScreenId = message.ScreenId,
            //            ButtonNameAR = message.ButtonNameAR,
            //            ButtonNameEN = message.ButtonNameEN,
            //            MessageAR = message.MessageAR,
            //            MessageEN = message.MessageEN,
            //            ButtonType = message.ButtonType
            //        };
            //        bool isDeleted = _deleteTicketRepository.Delete(request.ButtonId);
            //        if (isDeleted)
            //        {

            //            int generatedId = _addMessageRepository.Add(messageModel);
            //            return generatedId > 0;
            //        }
            //    }


            //    throw new NotSupportedException("Unsupported button type: " + request.ButtonType);
            //}

        }
        public bool DeleteButton(int buttonId)
        {
            try
            {
                bool isDeleted = _deleteButtonRepository.Delete(buttonId);
                return isDeleted;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected failure during Button Deletion process.");
                throw new DataAccessException("An unexpected structural error occurred. Please try again later.", ex);
            }
        }

    }
}
