using System.ComponentModel.DataAnnotations;

namespace Ticketing_Screen_Designer.DTO.Screens
{
    public class BaseScreenRequestDto
    {
        [Key]
        public int screenId { get; set; }
        [Required(ErrorMessage = "Screen name is required.")]
        [StringLength(100, ErrorMessage = "Screen name can't exeed 100 characters.")]
        public string ScreenName { get; set; }
        public bool IsActive { get; set; }


    }
}
