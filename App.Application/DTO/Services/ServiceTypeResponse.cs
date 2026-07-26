namespace App.Application.DTO.ServiceTypes
{
    public class ServiceTypeResponseDto
    {
        public int ServiceId { get; set; }
        public string ServicesName { get; set; }
        public string DisplayText
        {
            get
            {
                return $"{ServicesName}";
            }
        }
    }
}
