using System;
namespace Ticketing_Screen_Designer.DTO.Screens
{
    public class ScreenResponseDto
    {
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public int BankId { get; set; }
    }
}
