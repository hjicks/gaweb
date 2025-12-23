using MAS.Application.Dtos.MessageDtos;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MAS.ChatBot;

internal class Program
{
    private static readonly string masUrl = "https://localhost:7088";
    private static readonly string llmUrl = "http://127.0.0.1:1234";
    private static readonly string model = "LMStudioModel";
    private static readonly string tokenFilePath = "../../../token.txt";
    private static Timer _timer = null!;

    private static void Main()
    {
        Thread.Sleep(3000);
        string jwt, refreshToken;

        Console.WriteLine($"Authenticating as MASChatBot");
        Login("MASChatBot", "MASBOT1234", "ChatBot", "Windows 11");
        _timer = new Timer(RefreshSessionTask, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        Console.WriteLine($"Auth succeeded.");

        Console.WriteLine("Connecting to the hub, waiting for events...");
        var connectionSignalR = new HubConnectionBuilder().WithUrl(masUrl + "/hub", options =>
        {
            options.AccessTokenProvider = () => Task.FromResult<string?>(jwt);
        }).Build();
        connectionSignalR.StartAsync().Wait();

        connectionSignalR.On<MessageGetDto>("AddMessage", GetLlmResponse);

        bool running = true;
        while (running)
        {
            Console.Write("> ");
            string input = Console.ReadLine()!;
            IEnumerable<string> s = input.Split(" ").ToList();
            string cmd = s.First();

            switch (cmd)
            {
                case "exit":
                    running = false;
                    _timer.Dispose();
                    break;
            }
        }

        void Login(string username, string password, string clientName, string os)
        {
            HttpClient client = new() { BaseAddress = new Uri(masUrl) };
            var requestBody = new { username, password, clientName, os }; ;
            var result = client.PostAsJsonAsync("/api/sessions/login", requestBody).Result;
            var response = result.Content.ReadAsStringAsync().Result;

            if (result.IsSuccessStatusCode)
            {
                var masResponse = JsonSerializer.Deserialize<MasResponse>(response);
                jwt = masResponse!.Response.Jwt;
                refreshToken = masResponse.Response.RefreshToken;
                File.WriteAllText(tokenFilePath, refreshToken);
                Console.WriteLine("Logged in");
                return;
            }

            var masError = JsonSerializer.Deserialize<MasError>(response,
                new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter() } });
            if (masError!.Error == ErrorType.ActiveSessionAvailable)
            {
                refreshToken = File.ReadAllText(tokenFilePath).Trim();
                RefreshSession();
                return;
            }
            else { throw new Exception("Bot credentials are wrong."); }
        }

        void RefreshSession()
        {
            HttpClient client = new() { BaseAddress = new Uri(masUrl) };
            var response = client.PostAsync($"api/sessions/refresh/{refreshToken}", null).Result
                .Content.ReadAsStringAsync().Result;
            var masResponse = JsonSerializer.Deserialize<MasResponse>(response);
            jwt = masResponse!.Response.Jwt;
            refreshToken = masResponse.Response.RefreshToken;
            File.WriteAllText(tokenFilePath, refreshToken);
            Console.WriteLine($"Session refreshed at {DateTime.UtcNow}");
        }

        void RefreshSessionTask(object? state)
        {
            RefreshSession();
        }

        void GetLlmResponse(MessageGetDto msg)
        {
            Console.WriteLine($"\n====New msg=====\nFrom: {msg.SenderId}\nTo: {msg.DestinationId}\n" +
            $"Text: {msg.Text}");
            HttpClient client = new() { BaseAddress = new Uri(llmUrl) };
            var requestBody = new { model, input = msg.Text };
            var response = client.PostAsJsonAsync("/v1/responses", requestBody).Result
                .Content.ReadAsStringAsync().Result;
            var llmResponse = JsonSerializer.Deserialize<LlmResponse>(response);
            SendMessage(msg.DestinationId, llmResponse!.Output.Single().Content.Single().Text, jwt);
            Console.WriteLine($"LLM response: {llmResponse!.Output.Single().Content.Single().Text}");
        }

        void SendMessage(int destinationId, string text, string token)
        {
            HttpClient client = new() { BaseAddress = new Uri(masUrl) };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            MessageAddDto msg = new()
            {
                DestinationId = destinationId,
                Text = text,
            };
            var response = client.PostAsJsonAsync("/api/messages", msg).Result
                .Content.ReadAsStringAsync().Result;
        }
    }
}
