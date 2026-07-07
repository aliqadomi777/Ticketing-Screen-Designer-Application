using System;
namespace Ticketing_Screen_Designer.Models
{
    public class ButtonModel
    {
        public int ButtonId { get; set; }
        public string ButtonNameEN { get; set; }
        public string ButtonNameAR { get; set; }
        public int ButtonType { get; set; }
        public int ScreenId { get; set; }
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.UtcNow;
        public string TypeName { get; set; }
    }


}