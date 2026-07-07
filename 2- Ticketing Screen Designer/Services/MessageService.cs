using Ticketing_Screen_Designer.DTO.Messages;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Utils;
namespace Ticketing_Screen_Designer.Services
{
    public class MessageService : IMessageService
    {
        private readonly IAddableRepository<MessageModel> _addRepository;
        private readonly IUpdateableRepository<MessageModel> _updateRepository;
        private readonly IDeleteableRepository<MessageModel> _deleteRepository;
        public MessageService(IAddableRepository<MessageModel> addRepository,
            IUpdateableRepository<MessageModel> updateRepository,
            IDeleteableRepository<MessageModel> deleteRepository)
        {
            _addRepository = addRepository;
            _updateRepository = updateRepository;
            _deleteRepository = deleteRepository;
        }
        public int AddMessage(CreateMessageRequestDto request)
        {
            ValidationExtensions.ValidateModel(request);
            var messageModel = new MessageModel
            {
                ButtonId = request.ButtonId,
                MessageAR = request.MessageAR,
                MessageEN = request.MessageEN
            };
            int generatedId = _addRepository.Add(messageModel);
            return generatedId;
        }
        public bool UpdateMessage(UpdateMessageRequestDto request)
        {
            ValidationExtensions.ValidateModel(request);
            var messageModel = new MessageModel
            {
                MessageId = request.messageId,
                MessageAR = request.MessageAR,
                MessageEN = request.MessageEN
            };
            bool isUpdated = _updateRepository.Update(messageModel);
            return isUpdated;
        }
        public bool DeleteMessage(int messageId)
        {
            {
                bool isDeleted = _deleteRepository.Delete(messageId);
                return isDeleted;
            }
        }
    }
}