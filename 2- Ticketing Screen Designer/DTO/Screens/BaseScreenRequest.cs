using System.ComponentModel.DataAnnotations;

namespace Ticketing_Screen_Designer.DTO.Screens
{
    public class BaseScreenRequestDto
    {
        [Key]
        public int screenId { get; set; }
        [Required(ErrorMessage = "Screen name is required.")]
        [MinLength(1, ErrorMessage = "Screen name must be at least 1 characters long.")]
        [MaxLength(100, ErrorMessage = "Screen name can't exceed 100 characters.")]
        public string ScreenName { get; set; }
        public bool IsActive { get; set; }



    }
}
