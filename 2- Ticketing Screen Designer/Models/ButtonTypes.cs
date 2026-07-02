using System.ComponentModel.DataAnnotations;
namespace Ticketing_Screen_Designer.Models
{
    public class ButtonTypes
    {
        [Key]
        public int TypeId { get; set; }
        [Required(ErrorMessage = "Type Name is required.")]
        [StringLength(100, ErrorMessage = "Type Name can't exeed 100 characters.")]
        public string TypeName { get; set; }
    }
}