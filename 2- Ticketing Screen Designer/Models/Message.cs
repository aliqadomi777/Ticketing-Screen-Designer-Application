using System.ComponentModel.DataAnnotations;
namespace Ticketing_Screen_Designer.Models
{
    public class MessageModel : ButtonModel
    {
        [Required(ErrorMessage = "Button name is required.")]
        [StringLength(500, ErrorMessage = "Button name can't exeed 500 characters.")]
        public string MessageEN { get; set; }
        [Required(ErrorMessage = "Button name is required.")]
        [StringLength(500, ErrorMessage = "Button name can't exeed 500 characters.")]
        public string MessageAR { get; set; }
        [Required(ErrorMessage = "Message ID reference is required.")]
        public int MessageId { get; set; }
    }
}