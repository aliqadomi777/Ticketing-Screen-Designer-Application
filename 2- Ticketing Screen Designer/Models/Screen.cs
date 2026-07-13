using System;
namespace Ticketing_Screen_Designer.Models
{

    public class ScreenModel
    {
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.UtcNow;
        public int BankId { get; set; }
    }

}