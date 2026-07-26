using System.ComponentModel.DataAnnotations;

namespace App.Application.DTO.Messages
{
    public class CreateMessageRequestDto : BaseMessageRequestDto
    {
        [Required(ErrorMessage = "Button ID reference is required.")]
        public int ButtonId { get; set; }
    }
}


