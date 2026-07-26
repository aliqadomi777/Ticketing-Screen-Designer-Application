using System.ComponentModel.DataAnnotations;

namespace App.Application.DTO.Tickets
{
    public class BaseTicketRequestDto
    {
        [Required(ErrorMessage = "Service ID reference is required.")]
        public int ServiceId { get; set; }


    }
}
