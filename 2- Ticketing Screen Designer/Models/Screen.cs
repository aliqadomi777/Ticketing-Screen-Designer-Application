using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using Ticketing_Screen_Designer.Models;
namespace Ticketing_Screen_Designer
{

    public class Screen
    {
        [Key]
        public int ScreenId { get; set; }
        [Required(ErrorMessage = "Screen name is required.")]
        [StringLength(100, ErrorMessage = "Screen name cannot exceed 100 characters.")]
        public string ScreenName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.UtcNow;
        [Required(ErrorMessage = "Bank ID reference is required.")]
        public int BankId { get; set; }
    }

}