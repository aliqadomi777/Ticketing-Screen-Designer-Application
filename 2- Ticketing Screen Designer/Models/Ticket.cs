using System;
using System.ComponentModel.DataAnnotations;
namespace Ticketing_Screen_Designer.Models
{
    public class TicketModel : ButtonModel
    {

        [Required(ErrorMessage = "Ticket ID reference is required.")]
        public int TicketId { get; set; }
        [Required(ErrorMessage = "Service ID reference is required.")]
        public int ServiceId { get; set; }

        public String ServiceName { get; set; }

    }
}