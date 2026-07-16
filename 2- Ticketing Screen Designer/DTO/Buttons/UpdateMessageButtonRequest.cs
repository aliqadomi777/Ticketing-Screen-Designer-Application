
using System.ComponentModel.DataAnnotations;

namespace Ticketing_Screen_Designer.DTO.Buttons
{
    public class UpdateMessageButtonRequest : UpdateButtonRequestDto
    {
        [Key]
        [Required(ErrorMessage = "message ID key is required.")]
        public int messageId { get; set; }

        [Required(ErrorMessage = "MessageEN is required.")]
        [MinLength(1, ErrorMessage = "MessageEN  must be at least 1 characters long.")]
        [MaxLength(500, ErrorMessage = "MessageEN  can't exceed 500 characters.")]

        public string MessageEN { get; set; }
        [Required(ErrorMessage = "MessageAR is required.")]
        [MinLength(1, ErrorMessage = "MessageAR  must be at least 1 characters long.")]
        [MaxLength(500, ErrorMessage = "MessageAR  can't exceed 500 characters.")]

        public string MessageAR { get; set; }

        public int TicketId { get; set; }
    }
}
