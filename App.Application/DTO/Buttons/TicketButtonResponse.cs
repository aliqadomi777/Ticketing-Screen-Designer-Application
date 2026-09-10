using App.Application.DTO.ServiceTypes;

namespace App.Application.DTO.Buttons
{
    public class TicketButtonResponseDto : BaseButtonResponseDto
    {

        public int TicketId { get; set; }
        public int ServiceId { get; set; }
        public ServiceTypeResponseDto Service { get; set; }
    }
}