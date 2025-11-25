namespace MASsenger.Application.Dtos.Create
{
    public class SystemMessageCreateDto
    {
        public Int32 DestinationID { set; get; }
        public string Text { set; get; } = string.Empty;
    }
}
