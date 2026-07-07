using System.ComponentModel.DataAnnotations;

namespace Ticketing_Screen_Designer.DTO.Tickets
{
    public class CreateTicketRequestDto : BaseTicketRequestDto
    {
        [Required(ErrorMessage = "Button ID reference is required.")]
        public int ButtonId { get; set; }
    }
}
