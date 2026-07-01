
using System.ComponentModel.DataAnnotations;

namespace Ticketing_Screen_Designer
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        [Required(ErrorMessage = "Service ID reference is required.")]
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "Button ID reference is required.")]

        public int ButtonId { get; set; }

    }
}