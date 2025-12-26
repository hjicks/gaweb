using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Dtos.MessageDtos;
using MAS.Client;
using Microsoft.AspNetCore.SignalR.Client;

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

        Client c = new(baseurl, username, password, clientName, os);
        Console.WriteLine($"Authenticating as {username}");
        c.Login();

        Console.WriteLine($"Logged in");
        Console.WriteLine("Connecting to the hub, waiting for events...");
        var connectionSignalR = new HubConnectionBuilder().WithUrl(baseurl + "/hub", options =>
        {
            options.AccessTokenProvider = () => Task.FromResult<string?>(c.token);
        }).Build();
        connectionSignalR.StartAsync().Wait();

        connectionSignalR.On<MessageGetDto>("AddMessage",
            msg => Console.WriteLine($"\nu{msg.SenderId} -> d{msg.DestinationId}: {msg.Text}"));

        connectionSignalR.On<int>("AddGroupMemberCommand",
            gpid => Console.WriteLine($"Welcome to group {gpid}"));

        connectionSignalR.On<GroupChatMemberGetDto, int>("JoinGroupChat",
            (gcm, gid) => Console.WriteLine($"d{gid} -> u{gcm.MemberId} joins."));

        connectionSignalR.On<int, int>("LeaveGroupChat",
            (gpid, uid) => Console.WriteLine($"d{gpid} <- u{uid} parts."));
        
        connectionSignalR.On<int, bool>("BanOrUnbanGroupMemberCommand",
            (gpid, isBanned) => Console.WriteLine($"You are now {(isBanned ? "banned" : "unbanned")} from group {gpid}"));

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            IEnumerable<string> s = input!.Split(" ").ToList();
            string cmd = s.First();

            /*
             * this is the worst and yet most common form of parser.
             * I'm sorry.
             */
            switch (cmd)
            {
                case "/invite":
                    {
                        s = s.Skip(1);
                        int gp = Convert.ToInt32(s.First());
                        s = s.Skip(1);
                        int uid = Convert.ToInt32(s.First());
                        Console.WriteLine(c.Invite(gp, uid));
                        break;
                    }
                case "/list":
                    {
                        Console.WriteLine(c.List().ToString());
                        break;
                    }
                case "/lusers":
                    {
                        Console.WriteLine(c.Lusers().ToString());
                        break;
                    }
                case "/names":
                    {
                        s = s.Skip(1);
                        int gp = Convert.ToInt32(s.First());
                        Console.WriteLine(c.Names(gp).ToString());
                        break;
                    }
                case "/ban":
                    {
                        s = s.Skip(1);
                        int gp = Convert.ToInt32(s.First());
                        s = s.Skip(1);
                        int uid = Convert.ToInt32(s.First());
                        Console.WriteLine(c.Ban(gp, uid).ToString());
                        break;
                    }
                case "/part":
                    {
                        s = s.Skip(1);
                        int gp = Convert.ToInt32(s.First());
                        //Console.WriteLine(c.Leave().ToString());
                        c.Leave(gp);
                        break;
                    }
                case "/ref":
                    c.Refresh();
                    break;
                case "/msg":
                    {
                        s = s.Skip(1);
                        int dst = Convert.ToInt32(s.First());
                        string text = string.Join(" ", s.Skip(1));
                        c.SendMessage(dst, text);
                        break;
                    }
                case "":
                    /* Fall through */
                default:
                    break;
            }

        }
    }
}