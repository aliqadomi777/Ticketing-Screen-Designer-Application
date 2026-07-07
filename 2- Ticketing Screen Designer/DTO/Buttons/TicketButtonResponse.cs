namespace Ticketing_Screen_Designer.DTO.Buttons
{
    public class TicketButtonResponseDto : BaseButtonResponseDto
    {

        public int TicketId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
    }
}