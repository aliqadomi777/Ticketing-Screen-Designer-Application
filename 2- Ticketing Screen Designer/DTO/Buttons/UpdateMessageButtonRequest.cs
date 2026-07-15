
using System.ComponentModel.DataAnnotations;

namespace Ticketing_Screen_Designer.DTO.Buttons
{
    public class UpdateMessageButtonRequest : UpdateButtonRequestDto
    {
        [Key]
        [Required(ErrorMessage = "message ID key is required.")]
        public int messageId { get; set; }

        [Required(ErrorMessage = "AR Button name is required.")]
        [StringLength(500, ErrorMessage = "Button name can't exeed 500 characters.")]
        public string MessageEN { get; set; }
        [Required(ErrorMessage = "EN Button name is required.")]
        [StringLength(500, ErrorMessage = "Button name can't exeed 500 characters.")]
        public string MessageAR { get; set; }

        public int TicketId { get; set; }
    }
}
