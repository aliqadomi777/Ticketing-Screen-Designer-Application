using System.ComponentModel.DataAnnotations;
namespace Ticketing_Screen_Designer.DTO.Banks
{
    public class CreateBankRequestDto
    {
        [Required(ErrorMessage = "Bank name is required.")]
        [StringLength(100, ErrorMessage = "Bank name can't exeed 100 characters.", MinimumLength = 3)]
        public string BankName { get; set; }

    }
}
