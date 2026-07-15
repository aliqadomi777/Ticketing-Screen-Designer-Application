using System.ComponentModel.DataAnnotations;
namespace Ticketing_Screen_Designer.DTO.Banks
{
    public class CreateBankRequestDto
    {
        [Required(ErrorMessage = "Bank name is required.")]
        [MinLength(3, ErrorMessage = "Bank name must be at least 3 characters long.")]
        [MaxLength(100, ErrorMessage = "Bank name can't exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s.-]+$", ErrorMessage = "Bank name must contain only letters and spaces.")]
        public string BankName { get; set; }

    }
}
