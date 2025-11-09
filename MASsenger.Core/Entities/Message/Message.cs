namespace MASsenger.Core.Entities
{
    public class Message : BaseMessage
    {
        public BaseUser Sender { get; set; } = null!;

        // 1st is sent, 2nd is recivied, 3rd one is for seen
        public bool[] SeenMark = new bool[3];
    }
}
