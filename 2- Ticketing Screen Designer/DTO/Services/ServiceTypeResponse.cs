namespace Ticketing_Screen_Designer.DTO.Services
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
