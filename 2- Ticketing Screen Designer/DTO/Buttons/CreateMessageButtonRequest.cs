using System.ComponentModel.DataAnnotations;

namespace Ticketing_Screen_Designer.DTO.Buttons
{
    public class CreateMessageButtonRequestDto : BaseButtonDto
    {

        [Required(ErrorMessage = "MessageEN is required.")]
        [MinLength(1, ErrorMessage = "MessageEN  must be at least 1 characters long.")]
        [MaxLength(500, ErrorMessage = "MessageEN  can't exceed 500 characters.")]

        public string MessageEN { get; set; }
        [Required(ErrorMessage = "MessageAR is required.")]
        [MinLength(1, ErrorMessage = "MessageAR  must be at least 1 characters long.")]
        [MaxLength(500, ErrorMessage = "MessageAR  can't exceed 500 characters.")]

        public string MessageAR { get; set; }

        public string ButtonId { get; set; }
        public override string DisplayText
        {
            get
            {
                return base.DisplayText + $"Show Message (pending)";
            }
        }
    }
}
