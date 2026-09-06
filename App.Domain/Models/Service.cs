using System;
namespace App.Domain.Models
{
    public class ServiceModel
    {
        public int ServiceId { get; set; }

        public string ServiceNameEN { get; set; }

        public string ServiceNameAR { get; set; }

        public int MaxTicketsPerDay { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset ModifiedAt { get; set; }

        public int BankId { get; set; }

        public int MinimumServiceTime { get; set; }

        public int MaximumServiceTime { get; set; }
    }
}