namespace MAS.Core.Entities.MessageEntities
{
    public class FileContent
    {
        public int MessageId { get; set; }
        public byte[] Content { get; set; } = Array.Empty<byte>();

        // navigation properties

        // one-to-one
        public Message Message { get; set; } = new Message();
    }
}
