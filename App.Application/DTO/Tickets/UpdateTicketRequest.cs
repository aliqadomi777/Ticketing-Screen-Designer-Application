using System.ComponentModel.DataAnnotations;

namespace App.Application.DTO.Tickets

{
    public class UpdateTicketRequestDto : BaseTicketRequestDto
    {
        [Key]
        [Required(ErrorMessage = "Ticket ID key is required.")]
        public int TicketId { get; set; }
    }
}
