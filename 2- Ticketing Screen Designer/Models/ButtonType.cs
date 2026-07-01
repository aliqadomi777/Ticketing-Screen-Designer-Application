using System.ComponentModel.DataAnnotations;
namespace Ticketing_Screen_Designer.Models
{
    public class ButtonType
    {
        [Key]
        public string TypeId { get; set; }
        [Required(ErrorMessage = "Type name is required.")]
        [StringLength(100, ErrorMessage = "Type name can't exeed 100 characters.")]
        public string TypeName { get; set; }
    }
}
