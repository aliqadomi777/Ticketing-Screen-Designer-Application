using System;

namespace Ticketing_Screen_Designer.DTO.Buttons
{
    public class BaseButtonResponseDto : BaseButtonDto
    {
        public int ButtonId { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public string TypeName { get; set; }


        public string DisplayText
        {
            get
            {
                return $"{base.ButtonNameEN} - {base.ButtonNameAR} | {TypeName}";
            }
        }

    }
}
