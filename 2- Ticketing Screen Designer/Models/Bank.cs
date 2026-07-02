using System.ComponentModel.DataAnnotations;
namespace Ticketing_Screen_Designer.Models
{
    public class BankModel
    {
        [Key]
        public int BankId { get; set; }

        [Required(ErrorMessage = "Bank name is required.")]
        [StringLength(100, ErrorMessage = "Bank name can't exeed 100 characters.")]
        public string BankName { get; set; }
    }

}
