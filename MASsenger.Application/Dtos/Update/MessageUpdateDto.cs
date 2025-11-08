namespace MASsenger.Application.Dtos.Update
{
    public class MessageUpdateDto
    {
        public Int32 Id { get; }

        /* 
         * IRC doesn't offer this, why should we?
         * maybe we should remove it.
         */
        public string Text { get; set;  } = string.Empty;
    }
}
