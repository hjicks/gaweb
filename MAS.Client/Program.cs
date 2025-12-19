using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using MASsenger.Application.Dtos.MessageDtos;
using Microsoft.AspNetCore.SignalR.Client;

internal class Program
{
    private static void Main(string[] args)
    {
        string baseurl = "https://localhost:7088";
        JsonElement r;
        string username, password;

        JsonElement Login(string username, string password)
        {
            HttpClient c = new HttpClient { BaseAddress = new Uri(baseurl) };
            var msg = new { username, password };
            var response = c.PostAsJsonAsync("/api/session/login", msg).Result
                .Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<JsonElement>(response);
        }

        JsonElement SendMessage(int destinationId, string text, string token)
        {
            HttpClient c = new HttpClient { BaseAddress = new Uri(baseurl) };
            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var msg = new { destinationId, text };
            var response = c.PostAsJsonAsync("/api/chat/message", msg).Result
                .Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<JsonElement>(response);
        }

        JsonElement List(string token)
        {
            HttpClient c = new HttpClient { BaseAddress = new Uri(baseurl) };
            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = c.GetFromJsonAsync<JsonElement>("/api/ChannelChat").Result;
            return response;
        }

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
        Console.WriteLine($"Authenticating as {username}");
        r = Login(username, password);
        
        string jwt = r.GetProperty("response").GetProperty("jwt").ToString();
        Console.WriteLine($"Auth successed..., jwt token is {jwt}");
        Console.WriteLine("Connecting to the hub, waiting for events...");
        var connectionSignalR = new HubConnectionBuilder().WithUrl(baseurl + "/hub", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult<string?>(jwt);
            }).Build();
        connectionSignalR.StartAsync().Wait();

        connectionSignalR.On<MessageGetDto>("AddMessage",
            (msg) => Console.WriteLine($"\n====New msg=====\nFrom: {msg.SenderId}\nTo: {msg.DestinationId}\n" +
                    $"Text: {msg.Text}"));
            while (true)
            {
            Console.Write("> ");
            string input = Console.ReadLine();
            IEnumerable<string> s = input.Split(" ").ToList();
            string cmd = s.First();
            

            switch (cmd)
            {
                case "":
                    break;
                case "/list":
                    Console.WriteLine(List(jwt).ToString());
                    break;
                case "/ref":
                    break;
                case "/msg":
                    s = s.Skip(1);
                    goto default;
                default:
                    int dst = Convert.ToInt32(s.First());
                    string text = string.Join(" ", s.Skip(1));
                    SendMessage(dst, text, jwt);
                    break;
            }


        }
    }
}