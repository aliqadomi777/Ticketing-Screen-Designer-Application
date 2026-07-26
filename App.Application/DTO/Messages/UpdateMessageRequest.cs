using System.ComponentModel.DataAnnotations;

namespace App.Application.DTO.Messages
{
    public class UpdateMessageRequestDto : BaseMessageRequestDto
    {
        [Key]
        [Required(ErrorMessage = "message ID key is required.")]
        public int messageId { get; set; }
    }
}


