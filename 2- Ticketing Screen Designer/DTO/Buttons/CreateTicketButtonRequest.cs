using System.ComponentModel.DataAnnotations;

namespace Ticketing_Screen_Designer.DTO.Buttons
{
    public class CreateTicketButtonRequestDto : BaseButtonDto
    {


        public string ButtonId { get; set; }


        public int ServiceId { get; set; }
    }
}
