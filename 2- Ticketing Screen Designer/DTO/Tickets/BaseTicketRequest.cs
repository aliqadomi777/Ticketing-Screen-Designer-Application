using System.ComponentModel.DataAnnotations;

namespace Ticketing_Screen_Designer.DTO.Tickets
{
    public class BaseTicketRequestDto
    {
        [Required(ErrorMessage = "Service ID reference is required.")]
        public int ServiceId { get; set; }


    }
}
