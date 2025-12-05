namespace MASsenger.Application.Dtos.MessageDtos
{
    public class MessageUpdateDto
    {
        public int Id { get; }

        /* 
         * IRC doesn't offer this, why should we?
         * maybe we should remove it.
         */
        public string Text { get; set;  } = string.Empty;
    }
}
