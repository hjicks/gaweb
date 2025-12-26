using System.Text.Json;
using MAS.Application.Dtos.MessageDtos;
using Microsoft.AspNetCore.SignalR.Client;
using MAS.Client;

internal class Program
{
    private static void Main(string[] args)
    {
        string username, password, clientName, os, baseurl;

        baseurl = "https://localhost:7088";
        if (args.Length == 2)
        {
            username = args[0];
            password = args[1];
        }
        else
        {
            username = "Admin";
            password = "sysadmin";
        }
        clientName = "MASCli";
        os = System.Runtime.InteropServices.RuntimeInformation.OSDescription;

        Client c = new(baseurl, username, password);
        Console.WriteLine($"Authenticating as {username}");
        c.Login();

        Console.WriteLine($"Logged in");
        Console.WriteLine("Connecting to the hub, waiting for events...");
        var connectionSignalR = new HubConnectionBuilder().WithUrl(baseurl + "/hub", options =>
        {
            options.AccessTokenProvider = () => Task.FromResult<string?>(c.Token);
        }).Build();
        connectionSignalR.StartAsync().Wait();

        connectionSignalR.On<MessageGetDto>("AddMessage",
            (msg) => Console.WriteLine($"\nS{msg.SenderId} -> D{msg.DestinationId}: {msg.Text}\n" +
            $"Text: {msg.Text}"));
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            IEnumerable<string> s = input!.Split(" ").ToList();
            string cmd = s.First();


            switch (cmd)
            {
                case "":
                    break;
                case "/list":
                    Console.WriteLine(c.List().ToString());
                    break;
                case "/ref":
                    break;
                case "/msg":
                    s = s.Skip(1);
                    goto default;
                default:
                    int dst = Convert.ToInt32(s.First());
                    string text = string.Join(" ", s.Skip(1));
                    c.SendMessage(dst, text);
                    break;
            }

        }
    }
}