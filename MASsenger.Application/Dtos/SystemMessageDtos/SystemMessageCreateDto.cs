namespace MASsenger.Application.Dtos.SystemMessageDtos
{
    public class SystemMessageCreateDto
    {
        public int DestinationID { set; get; }
        public string Text { set; get; } = string.Empty;
    }
}
