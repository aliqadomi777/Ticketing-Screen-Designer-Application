using System.Collections.Generic;
using Ticketing_Screen_Designer.DTO.Tickets;

namespace Ticketing_Screen_Designer.Interfaces.Services
{
    public interface ITicketService
    {
        int AddTicket(CreateTicketRequestDto request);
        bool UpdateTicket(UpdateTicketRequestDto request);
        bool DeleteTicket(int id);
    }
}