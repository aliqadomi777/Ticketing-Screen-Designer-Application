using System.ComponentModel.DataAnnotations;

namespace Ticketing_Screen_Designer.DTO.Messages
{
    public class CreateMessageRequestDto : BaseMessageRequestDto
    {
        [Required(ErrorMessage = "Button ID reference is required.")]
        public int ButtonId { get; set; }
    }
}


