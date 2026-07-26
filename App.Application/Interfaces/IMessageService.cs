using App.Application.DTO.Messages;

namespace App.Application.Interfaces
{
    public interface IMessageService
    {
        int AddMessage(CreateMessageRequestDto request);
        bool UpdateMessage(UpdateMessageRequestDto request);
        bool DeleteMessage(int id);
    }
}