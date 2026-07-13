//using Serilog;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Ticketing_Screen_Designer.DTO.Buttons;
//using Ticketing_Screen_Designer.Interfaces.Repositories;
//using Ticketing_Screen_Designer.Interfaces.Services;
//using Ticketing_Screen_Designer.Models;
//using Ticketing_Screen_Designer.Utils;

//namespace Ticketing_Screen_Designer.Services
//{
//    public class ButtonService : IButtonService
//    {
//        private readonly IButtonRepository<ButtonModel> _fetchButtonRepository;
//        private readonly IUpdateableRepository<ButtonModel> _updateButtonRepository;
//        private readonly IDeleteableRepository<ButtonModel> _deleteButtonRepository;
//        private readonly IListableRepository<ButtonModel> _listButtonRepository;
//        private readonly IAddableRepository<ButtonModel> _addButtonRepository;

//        private readonly IDeleteableRepository<TicketModel> _deleteTicketRepository;
//        private readonly IAddableRepository<TicketModel> _addTicketRepository;
//        private readonly ITicketRepository<TicketModel> _updateTicketRepository;

//        private readonly IDeleteableRepository<MessageModel> _deleteMessageRepository;
//        private readonly IAddableRepository<MessageModel> _addMessageRepository;
//        private readonly ITicketRepository<MessageModel> _updateMessageRepository;
//        public ButtonService(IButtonRepository<ButtonModel> fetchButtonRepository,
//            IAddableRepository<ButtonModel> addButtonRepository,
//            IUpdateableRepository<ButtonModel> updateButtonRepository,
//            IDeleteableRepository<ButtonModel> deleteButtonRepository,
//            IListableRepository<ButtonModel> listButtonRepository,
//            IDeleteableRepository<TicketModel> deleteTicketRepository,
//            IAddableRepository<TicketModel> addTicketRepository,
//            ITicketRepository<TicketModel> updateTicketRepository,
//            IDeleteableRepository<MessageModel> deleteMessageRepository,
//            IAddableRepository<MessageModel> addMessageRepository,
//            ITicketRepository<MessageModel> updateMessageRepository
//            )
//        {
//            _fetchButtonRepository = fetchButtonRepository;
//            _updateButtonRepository = updateButtonRepository;
//            _deleteButtonRepository = deleteButtonRepository;
//            _listButtonRepository = listButtonRepository;
//            _addButtonRepository = addButtonRepository;
//            _deleteTicketRepository = deleteTicketRepository;
//            _addTicketRepository = addTicketRepository;
//            _updateTicketRepository = updateTicketRepository;
//            _deleteMessageRepository = deleteMessageRepository;
//            _updateMessageRepository = updateMessageRepository;
//            _addMessageRepository = addMessageRepository;
//        }

//        public BaseButtonResponseDto GetButtonDetails(int buttonId, int buttonType)
//        {
//            if (buttonId <= 0)
//            {
//                throw new ArgumentException("button ID must be a positive non-zero integer.", nameof(buttonId));
//            }
//            if (buttonType <= 0)
//            {
//                throw new ArgumentException("button type must be a positive non-zero integer.", nameof(buttonType));
//            }
//            try
//            {
//                var button = _fetchButtonRepository.GetById(buttonId, buttonType);
//                if (button == null)
//                {
//                    return null;
//                }
//                if (button is TicketModel)
//                {
//                    TicketModel ticketModel = (TicketModel)button;
//                    return new TicketButtonResponseDto
//                    {
//                        ButtonId = ticketModel.ButtonId,
//                        ButtonNameAR = ticketModel.ButtonNameAR,
//                        ButtonNameEN = ticketModel.ButtonNameEN,
//                        ButtonType = ticketModel.ButtonType,
//                        ScreenId = ticketModel.ScreenId,
//                        ModifiedAt = ticketModel.ModifiedAt,
//                        ServiceId = ticketModel.ServiceId,
//                        ServiceName = ticketModel.ServiceName,
//                        TicketId = ticketModel.TicketId,
//                        TypeName = ticketModel.TypeName

//                    };
//                }
//                else if (button is MessageModel)
//                {
//                    MessageModel messageModel = (MessageModel)button;
//                    return new MessageButtonResponseDto
//                    {
//                        ButtonId = messageModel.ButtonId,
//                        ButtonNameAR = messageModel.ButtonNameAR,
//                        ButtonNameEN = messageModel.ButtonNameEN,
//                        ButtonType = messageModel.ButtonType,
//                        ScreenId = messageModel.ScreenId,
//                        ModifiedAt = messageModel.ModifiedAt,
//                        TypeName = messageModel.TypeName,
//                        MessageAR = messageModel.MessageAR,
//                        MessageEN = messageModel.MessageEN,
//                        MessageId = messageModel.MessageId
//                    };
//                }
//                return null;
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex, "Failed executing query for button with ID: {buttonId}", buttonId);
//                throw new DataAccessException($"Could not retrieve profile records for button ID {buttonId}.", ex);
//            }
//        }
//        public List<BaseButtonResponseDto> GetAllButtonsDetails(int screenId)
//        {
//            try
//            {
//                var buttons = _listButtonRepository.GetAll(screenId);
//                return buttons.Select(button => new BaseButtonResponseDto
//                {
//                    ButtonId = button.ButtonId,
//                    ScreenId = button.ScreenId,
//                    ButtonNameAR = button.ButtonNameAR,
//                    ButtonNameEN = button.ButtonNameEN,
//                    ButtonType = button.ButtonType,
//                    ModifiedAt = button.ModifiedAt,
//                    TypeName = button.TypeName
//                }).ToList();
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex, "Failed executing query for all buttons in screen with ID: {screenId}", screenId);
//                throw new DataAccessException($"Could not retrieve profile records for screen ID {screenId}.", ex);
//            }


//        }
//        public int AddButton(BaseButtonDto request)
//        {
//            try
//            {
//                if (request.ButtonType == 1)
//                {
//                    var ticket = (CreateTicketButtonRequestDto)request;
//                    ValidationExtensions.ValidateModel(ticket);
//                    var ticketModel = new TicketModel
//                    {
//                        ScreenId = ticket.ScreenId,
//                        ServiceId = ticket.ServiceId,
//                        ButtonNameEN = ticket.ButtonNameEN,
//                        ButtonNameAR = ticket.ButtonNameAR,
//                        ButtonType = ticket.ButtonType
//                    };
//                    int generatedId = _addTicketRepository.Add(ticketModel);
//                    return generatedId;
//                }
//                else if (request.ButtonType == 2)
//                {
//                    var message = (CreateMessageButtonRequestDto)request;
//                    ValidationExtensions.ValidateModel(message);
//                    var messageModel = new MessageModel
//                    {
//                        ScreenId = message.ScreenId,
//                        ButtonNameAR = message.ButtonNameAR,
//                        ButtonNameEN = message.ButtonNameEN,
//                        MessageAR = message.MessageAR,
//                        MessageEN = message.MessageEN,
//                        ButtonType = message.ButtonType
//                    };
//                    int generatedId = _addMessageRepository.Add(messageModel);
//                    return generatedId;
//                }
//            }



//            return 0;
//        }
//        public bool UpdateButton(UpdateButtonRequestDto request)
//        {
//            ValidationExtensions.ValidateModel(request);
//            var currentButton = _fetchRepository.GetById(request.ButtonId, request.ButtonType);
//            if (request.ButtonType == currentButton.ButtonType)
//            {
//                var buttonModel = new ButtonModel
//                {
//                    ButtonNameAR = currentButton.ButtonNameAR,
//                    ButtonNameEN = currentButton.ButtonNameEN,
//                    ButtonType = currentButton.ButtonType,
//                    ButtonId = currentButton.ButtonId,
//                };
//                bool isUpdated = _updateRepository.Update(buttonModel);
//                return isUpdated;

//            }
//            //changed the type delete then add
//            else if (request.ButtonType == 2)
//            {
//                //check button type


//            }
//            return false;
//        }
//        public bool DeleteButton(int buttonId)
//        {
//            try
//            {
//                bool isDeleted = _deleteRepository.Delete(buttonId);
//                return isDeleted;
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex, "Unexpected failure during Button Deletion process.");
//                throw new DataAccessException("An unexpected structural error occurred. Please try again later.", ex);
//            }
//        }

//    }
//}
