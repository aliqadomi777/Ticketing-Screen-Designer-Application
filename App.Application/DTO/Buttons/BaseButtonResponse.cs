using System;

namespace App.Application.DTO.Buttons
{
    public class BaseButtonResponseDto : BaseButtonDto
    {
        public int ButtonId { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public string TypeName { get; set; }


        public override string DisplayText
        {
            get
            {
                return $"{base.ButtonNameEN} - {base.ButtonNameAR} | {TypeName}";
            }
        }

    }
}
