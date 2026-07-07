using System.ComponentModel.DataAnnotations;

namespace Ticketing_Screen_Designer.DTO.Screens
{
    public class CreateScreenRequestDto : BaseScreenRequestDto
    {

        [Required(ErrorMessage = "Bank ID reference is required.")]
        public int BankId { get; set; }

    }
}
