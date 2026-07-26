using System.ComponentModel.DataAnnotations;

namespace App.Application.DTO.Screens
{
    public class CreateScreenRequestDto : BaseScreenRequestDto
    {

        [Required(ErrorMessage = "Bank ID reference is required.")]
        public int BankId { get; set; }

    }
}
