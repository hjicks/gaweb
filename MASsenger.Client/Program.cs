using Microsoft.AspNetCore.SignalR.Client;

var connectionSignalR = new HubConnectionBuilder().WithUrl("https://localhost:7088/hub").Build();
connectionSignalR.StartAsync().Wait();

Console.WriteLine("Connected, waiting for events...");
while (true)
{
    Thread.Sleep(1000);
    connectionSignalR.On<string>("RecieveText", text => Console.WriteLine(text));
}