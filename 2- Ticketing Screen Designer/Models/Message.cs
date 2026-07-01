using System.ComponentModel.DataAnnotations;
namespace Ticketing_Screen_Designer.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        [Required(ErrorMessage = "English message text is required.")]
        [StringLength(500, ErrorMessage = "English message cannot exceed 500 characters.")]
        public string MessageEN { get; set; }
        [Required(ErrorMessage = "Arabic message text is required.")]
        [StringLength(500, ErrorMessage = "Arabic message cannot exceed 500 characters.")]
        public string MessageAR { get; set; }
        [Required(ErrorMessage = "Button ID reference is required.")]
        public int ButtonId { get; set; }
    }
}