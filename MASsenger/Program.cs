namespace MASsenger
{
    class Program
    {

        static void Main()
        {
            using var context = new MessengerContext();

            Console.WriteLine("Reading data from database...\n");

            // Get the first channel
            var channel = context.Channels.FirstOrDefault();

            if (channel != null)
            {
                Console.WriteLine("Channel Details:");
                Console.WriteLine($"Id: {channel.Id}");
                Console.WriteLine($"Name: {channel.Name}");
                Console.WriteLine($"Description: {channel.Description}");
                Console.WriteLine($"Created At: {channel.CreatedAt}");
                Console.WriteLine($"Is Public: {channel.IsPublic}\n");
            }
            else
            {
                Console.WriteLine("No channels found in the database.");
            }
        }

    }
}