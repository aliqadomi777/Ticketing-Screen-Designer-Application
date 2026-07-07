using System.Collections.Generic;
using Ticketing_Screen_Designer.DTO.Messages;

namespace Ticketing_Screen_Designer.Interfaces.Services
{
    public interface IMessageService
    {
        int AddMessage(CreateMessageRequestDto request);
        bool UpdateMessage(UpdateMessageRequestDto request);
        bool DeleteMessage(int id);
    }
}