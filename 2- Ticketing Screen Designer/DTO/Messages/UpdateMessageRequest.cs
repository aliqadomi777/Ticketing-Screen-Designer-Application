using System.ComponentModel.DataAnnotations;

namespace Ticketing_Screen_Designer.DTO.Messages
{
    public class UpdateMessageRequestDto : BaseMessageRequestDto
    {
        [Key]
        [Required(ErrorMessage = "message ID key is required.")]
        public int messageId { get; set; }
    }
}


