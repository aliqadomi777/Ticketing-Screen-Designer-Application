using System;

namespace App.Application.DTO.ServiceTypes
{
    public class ServiceTypeResponseDto
    {
        public int ServiceId { get; set; }
        public string ServiceNameEN { get; set; }

        public string ServiceNameAR { get; set; }

        public int MaxTicketsPerDay { get; set; }

        public bool IsActive { get; set; }

        public int MinimumServiceTime { get; set; }

        public int MaximumServiceTime { get; set; }
        public string DisplayText
        {
            get
            {
                return $"{ServiceNameEN}";
            }
        }
    }
}
