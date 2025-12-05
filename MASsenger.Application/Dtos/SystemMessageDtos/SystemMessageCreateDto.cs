namespace MASsenger.Application.Dtos.SystemMessageDtos
{
    public record SystemMessageCreateDto
    {
        public int DestinationID { set; get; }
        public string Text { set; get; } = string.Empty;
    }
}
