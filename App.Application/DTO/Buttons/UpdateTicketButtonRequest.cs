using System.ComponentModel.DataAnnotations;

namespace App.Application.DTO.Buttons
{
    public class UpdateTicketButtonRequest : UpdateButtonRequestDto
    {

        public int TicketId { get; set; }

        [Required(ErrorMessage = "Service ID reference is required.")]
        public int ServiceId { get; set; }

        public int messageId { get; set; }

        public override string DisplayText
        {
            get
            {
                return base.DisplayText + $"Issue Ticket (pending)";
            }
        }

    }
}
