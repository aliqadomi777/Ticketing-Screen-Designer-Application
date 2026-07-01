using System;
using System.ComponentModel.DataAnnotations;
namespace Ticketing_Screen_Designer.Models
{
    public class Button
    {
        [Key]
        public int ButtonId { get; set; }
        [Required(ErrorMessage = "Button name is required.")]
        [StringLength(100, ErrorMessage = "Button name can't exeed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s-_]*$", ErrorMessage = "English name must contain English characters only.")]
        public string ButtonNameEN { get; set; }

        [Required(ErrorMessage = "Button name is required.")]
        [StringLength(100, ErrorMessage = "Button name can't exeed 100 characters.")]
        public string ButtonNameAR { get; set; }
        [Required(ErrorMessage = "ButtonType reference is required.")]
        public int ButtonType { get; set; }

        [Required(ErrorMessage = "Screen ID reference is required.")]
        public int ScreenId { get; set; }
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.UtcNow;
    }


}