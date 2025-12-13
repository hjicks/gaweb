using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.AspNetCore.SignalR.Client;

string baseurl = "https://localhost:7088";
JsonElement Login(string username, string password)
{
    HttpClient c = new HttpClient { BaseAddress = new Uri(baseurl) };
    var loginData = new { username = username, password = password };
    var response = c.PostAsJsonAsync("/api/session/login", loginData).Result
        .Content.ReadAsStringAsync().Result;
    return JsonSerializer.Deserialize<JsonElement>(response);
}
Console.WriteLine("Authenticating...");
JsonElement r = Login("Admin", "sysadmin");
String jwt = r.GetProperty("response").GetProperty("jwt").ToString();
Console.WriteLine($"Auth successed..., jwt token is {jwt}");
Console.WriteLine("Connecting to the hub, waiting for events...");
var connectionSignalR = new HubConnectionBuilder().WithUrl(baseurl + "/hub", options =>
    {
        options.AccessTokenProvider = () => Task.FromResult(jwt);
    }).Build();
connectionSignalR.StartAsync().Wait();

while (true)
{
    Thread.Sleep(1000);
    connectionSignalR.On<string, string>("a", (text, text2) => Console.WriteLine(text+text2));
}