using System.ComponentModel.DataAnnotations;

namespace App.Application.DTO.Tickets
{
    public class CreateTicketRequestDto : BaseTicketRequestDto
    {
        [Required(ErrorMessage = "Button ID reference is required.")]
        public int ButtonId { get; set; }
    }
}
