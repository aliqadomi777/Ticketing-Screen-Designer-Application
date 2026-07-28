using System;
namespace App.Domain.Models
{

    public class ScreenModel
    {
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public int BankId { get; set; }
    }

}