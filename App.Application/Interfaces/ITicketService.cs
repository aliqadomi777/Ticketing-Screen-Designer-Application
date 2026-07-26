using App.Application.DTO.Tickets;

namespace App.Application.Interfaces
{
    public interface ITicketService
    {
        int AddTicket(CreateTicketRequestDto request);
        bool UpdateTicket(UpdateTicketRequestDto request);
        bool DeleteTicket(int id);
    }
}