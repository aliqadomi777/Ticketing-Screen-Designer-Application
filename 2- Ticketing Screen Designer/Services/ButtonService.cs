using System.Collections.Generic;
using Ticketing_Screen_Designer.DTO.Buttons;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Utils;

namespace Ticketing_Screen_Designer.Services
{
    public class ButtonService : IButtonService
    {
        private readonly IButtonRepository<ButtonModel> _fetchRepository;
        private readonly IAddableRepository<MessageModel> _addMessageRepository;
        private readonly IAddableRepository<TicketModel> _addTicketRepository;
        private readonly IUpdateableRepository<ButtonModel> _updateRepository;
        private readonly IDeleteableRepository<ButtonModel> _deleteRepository;
        private readonly IListableRepository<ButtonModel> _listRepository;
        public ButtonService(IButtonRepository<ButtonModel> fetchRepository,
            IAddableRepository<MessageModel> addMessageRepository,
            IAddableRepository<TicketModel> addTicketRepository,
            IUpdateableRepository<ButtonModel> updateRepository,
            IDeleteableRepository<ButtonModel> deleteRepository,
            IListableRepository<ButtonModel> listRepository)
        {
            _fetchRepository = fetchRepository;
            _addMessageRepository = addMessageRepository;
            _addTicketRepository = addTicketRepository;
            _updateRepository = updateRepository;
            _deleteRepository = deleteRepository;
            _listRepository = listRepository;
        }

        public BaseButtonResponseDto GetButtonDetails(int buttonId, int buttonType)
        {
            var button = _fetchRepository.GetById(buttonId, buttonType);
            if (button == null)
            {
                return null;
            }
            if (button is TicketModel)
            {
                TicketModel ticket = (TicketModel)button;
                return new TicketButtonResponseDto
                {
                    ButtonId = ticket.ButtonId,
                    ButtonNameAR = ticket.ButtonNameAR,
                    ButtonNameEN = ticket.ButtonNameEN,
                    ButtonType = ticket.ButtonType,
                    ScreenId = ticket.ScreenId,
                    ModifiedAt = ticket.ModifiedAt,
                    ServiceId = ticket.ServiceId,
                    ServiceName = ticket.ServiceName,
                    TicketId = ticket.TicketId,
                    TypeName = ticket.TypeName

                };
            }
            else if (button is MessageModel)
            {
                MessageModel message = (MessageModel)button;
                return new MessageButtonResponseDto
                {
                    ButtonId = message.ButtonId,
                    ButtonNameAR = message.ButtonNameAR,
                    ButtonNameEN = message.ButtonNameEN,
                    ButtonType = message.ButtonType,
                    ScreenId = message.ScreenId,
                    ModifiedAt = message.ModifiedAt,
                    TypeName = message.TypeName,
                    MessageAR = message.MessageAR,
                    MessageEN = message.MessageEN,
                    MessageId = message.MessageId
                };
            }
            return null;
        }
        public List<BaseButtonResponseDto> GetAllButtonsDetails(int screenId)
        {
            var buttonsPerBank = new List<BaseButtonResponseDto>();
            var buttons = _listRepository.GetAll(screenId);

            foreach (var button in buttons)
            {
                buttonsPerBank.Add(new BaseButtonResponseDto
                {
                    ButtonId = button.ButtonId,
                    ScreenId = button.ScreenId,
                    ButtonNameAR = button.ButtonNameAR,
                    ButtonNameEN = button.ButtonNameEN,
                    ButtonType = button.ButtonType,
                    ModifiedAt = button.ModifiedAt,
                    TypeName = button.TypeName
                });

            }

            return buttonsPerBank;
        }
        public int AddButton(BaseButtonDto request)
        {
            //reflect on button types here and in repo and models and dto as enums (1:ticket , 2:message)
            int generatedId = 0;
            if (request.ButtonType == 1)
            {
                var ticket = (CreateTicketButtonRequestDto)request;
                ValidationExtensions.ValidateModel(ticket);
                var ticketModel = new TicketModel
                {
                    ScreenId = ticket.ScreenId,
                    ServiceId = ticket.ServiceId,
                    ButtonNameEN = ticket.ButtonNameEN,
                    ButtonNameAR = ticket.ButtonNameAR,
                    ButtonType = ticket.ButtonType
                };
                generatedId = _addTicketRepository.Add(ticketModel);
                return generatedId;
            }
            else if (request.ButtonType == 2)
            {
                var message = (CreateMessageButtonRequestDto)request;
                ValidationExtensions.ValidateModel(message);
                var messageModel = new MessageModel
                {
                    ScreenId = message.ScreenId,
                    ButtonNameAR = message.ButtonNameAR,
                    ButtonNameEN = message.ButtonNameEN,
                    MessageAR = message.MessageAR,
                    MessageEN = message.MessageEN,
                    ButtonType = message.ButtonType
                };
                generatedId = _addMessageRepository.Add(messageModel);
                return generatedId;
            }


            return 0;
        }
        public bool UpdateButton(UpdateButtonRequestDto request)
        {
            ValidationExtensions.ValidateModel(request);
            var currentButton = _fetchRepository.GetById(request.ButtonId, request.ButtonType);
            if (request.ButtonType == currentButton.ButtonType)
            {
                var buttonModel = new ButtonModel
                {
                    ButtonNameAR = currentButton.ButtonNameAR,
                    ButtonNameEN = currentButton.ButtonNameEN,
                    ButtonType = currentButton.ButtonType,
                    ButtonId = currentButton.ButtonId,
                };
                bool isUpdated = _updateRepository.Update(buttonModel);
                return isUpdated;

            }
            //changed the type delete then add
            else if (request.ButtonType == 2)
            {
                //check button type
                //when i inject through interfaces which will be really used ? cuz we are calling the message service and ticketservice

            }
            return false;
        }
        public bool DeleteButton(int buttonId)
        {
            bool isDeleted = _deleteRepository.Delete(buttonId);
            return isDeleted;
        }
    }
}
