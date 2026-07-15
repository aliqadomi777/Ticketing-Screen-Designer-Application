using System.ComponentModel.DataAnnotations;

namespace Ticketing_Screen_Designer.DTO.Buttons
{
    public class UpdateButtonRequestDto : BaseButtonDto
    {
        [Required(ErrorMessage = "Button ID reference is required.")]
        public int ButtonId { get; set; }

    }
}
