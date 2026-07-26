using App.Application.DTO.Messages;
using App.Application.Interfaces;
using App.Domain.Interfaces;
using App.Domain.Models;
using App.Shared;
namespace App.Application.Services
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