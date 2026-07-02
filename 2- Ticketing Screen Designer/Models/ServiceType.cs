using System.ComponentModel.DataAnnotations;
namespace Ticketing_Screen_Designer.Models
{
    public class ServiceType
    {
        [Key]
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "Service Name is required.")]
        [StringLength(100, ErrorMessage = "Service Name can't exeed 100 characters.")]
        public string ServicesName { get; set; }
    }
}