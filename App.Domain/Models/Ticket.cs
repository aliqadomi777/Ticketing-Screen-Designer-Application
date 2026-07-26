namespace App.Domain.Models
{
    public class TicketModel : ButtonModel
    {
        public int TicketId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }

    }
}