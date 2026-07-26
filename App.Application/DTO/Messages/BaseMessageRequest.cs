
using System.ComponentModel.DataAnnotations;

namespace App.Application.DTO.Messages
{
    public class BaseMessageRequestDto
    {
        [Required(ErrorMessage = "AR Button name is required.")]
        [StringLength(500, ErrorMessage = "Button name can't exeed 500 characters.")]
        public string MessageEN { get; set; }
        [Required(ErrorMessage = "EN Button name is required.")]
        [StringLength(500, ErrorMessage = "Button name can't exeed 500 characters.")]
        public string MessageAR { get; set; }

    }
}
