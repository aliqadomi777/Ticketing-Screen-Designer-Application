using System.ComponentModel.DataAnnotations;

namespace Ticketing_Screen_Designer.DTO.Tickets
{
    public class UpdateTicketRequestDto : BaseTicketRequestDto
    {
        [Key]
        [Required(ErrorMessage = "Ticket ID key is required.")]
        public int TicketId { get; set; }
    }
}
