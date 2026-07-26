using System.ComponentModel.DataAnnotations;

namespace App.Application.DTO.Buttons
{
    public class BaseButtonDto
    {


        [Required(ErrorMessage = "Button English name is required.")]
        [MinLength(1, ErrorMessage = "Button English name  must be at least 1 characters long.")]
        [MaxLength(100, ErrorMessage = "Button English name  can't exceed 100 characters.")]
        public string ButtonNameEN { get; set; }

        [Required(ErrorMessage = "Button Arabic name is required.")]
        [MinLength(1, ErrorMessage = "Button Arabic name  must be at least 1 characters long.")]
        [MaxLength(100, ErrorMessage = "Button Arabic name  can't exceed 100 characters.")]
        public string ButtonNameAR { get; set; }

        public int ButtonType { get; set; }
        public int ScreenId { get; set; }


        public virtual string DisplayText
        {
            get
            {
                return $"{ButtonNameEN} - {ButtonNameAR} | ";
            }
        }

    }
}
