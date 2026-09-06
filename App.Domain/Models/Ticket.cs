namespace App.Domain.Models
{
    public class TicketModel : ButtonModel
    {
        public int TicketId { get; set; }
        public int ServiceId { get; set; }
        public ServiceModel Service { get; set; }
    }
}