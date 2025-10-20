using System;

namespace MASsenger
{
    class Program
    {
        private static void Main()
        {
            while (true)
            {
                Console.WriteLine("You can perform CRUD opearions. Which one do you prefer? (Enter c, r, u, d or e to exit.)");
                string? op = Console.ReadLine();
                using var context = new MessengerContext();
                var channel = new ChannelGroupChat();

                switch (op)
                {
                    case "c":
                        Console.WriteLine("Enter channel name:");
                        channel.Name = Console.ReadLine()!;
                        Console.WriteLine("Enter channel description:");
                        channel.Description = Console.ReadLine();
						
                        context.Add(channel);
                        context.SaveChanges();
                        Console.WriteLine();
                        break;

                    case "r":
                        Console.WriteLine("Reading data from database...\n");
                        //var channels = context.Channels.ToList();
                        //if (channels.Count != 0)
                        //{
                        //    Console.WriteLine("Channel Details:\n");
                        //    foreach (var item in channels)
                        //    {
                        //        Console.WriteLine($"Id: {item.Id}");
                        //        Console.WriteLine($"Name: {item.Name}");
                        //        Console.WriteLine($"Description: {item.Description}");
                        //        Console.WriteLine($"Created At: {item.CreationTime}");
                        //    }
                        //}
                        //else
                        //{
                        //    Console.WriteLine("No channels found in the database.\n");
                        //}
                        break;

                    case "u":
                        Console.WriteLine("Enter channel Id:");
                        channel.Id = UInt64.Parse(Console.ReadLine()!);
                        Console.WriteLine("Enter channel name:");
                        channel.Name = Console.ReadLine()!;
                        Console.WriteLine("Enter channel description:");;
                        context.Update(channel);
                        context.SaveChanges();
                        Console.WriteLine();
                        break;

                    case "d":
                        Console.WriteLine("Enter the channel Id that you want to remove:");
                        channel.Id = UInt64.Parse(Console.ReadLine()!);
                        context.Remove(channel);
                        context.SaveChanges();
                        Console.WriteLine();
                        break;

                    default:
                        if (op != "e") Console.WriteLine("Incorrect operation.\n");
                        break;
                }

                if (op == "e") break;
            }

        }

    }
}
