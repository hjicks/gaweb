namespace MASsenger.Application.Dtos.Read
{
    public class MessageUpdateDto
    {
        /* 
         * IRC doesn't offer this, why should we?
         * maybe we should remove it.
         */
        public string? Text { get; }
    }
}
