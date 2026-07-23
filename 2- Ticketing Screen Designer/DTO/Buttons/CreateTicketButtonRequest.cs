namespace Ticketing_Screen_Designer.DTO.Buttons
{
    public class CreateTicketButtonRequestDto : BaseButtonDto
    {


        public string ButtonId { get; set; }


        public int ServiceId { get; set; }

        public override string DisplayText
        {
            get
            {
                return base.DisplayText + $"Issue Ticket (pending)";
            }
        }
    }
}
