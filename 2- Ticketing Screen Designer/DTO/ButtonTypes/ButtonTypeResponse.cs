namespace Ticketing_Screen_Designer.DTO.ButtonTypes
{
    public class ButtonTypeResponseDto
    {

        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string DisplayText
        {
            get
            {
                return $"{TypeName}";
            }
        }
    }
}
