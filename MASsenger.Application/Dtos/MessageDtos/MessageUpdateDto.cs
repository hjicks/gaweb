namespace MASsenger.Application.Dtos.MessageDtos
{
    public record MessageUpdateDto
    {
        public int Id { get; set; }

        /* 
         * IRC doesn't offer this, why should we?
         * maybe we should remove it.
         */
        public string Text { get; set;  } = string.Empty;
    }
}
