namespace MASsenger.Application.Dto.Update
{
    public class MessageUpdateDto
    {
        public UInt64 Id { get; set; }

        /* 
         * IRC doesn't offer this, why should we?
         * maybe we should remove it.
         */
        public string? Text { get; set;  }
    }
}
