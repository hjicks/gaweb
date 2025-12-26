using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using MAS.Application.Dtos.MessageDtos;

namespace MAS.Client;
public class Client
{
    private readonly string _url;
    private readonly string _username;
    private readonly string _passwd;
    public string? /* JWT */ Token;
    public Client(string url, string username, string passwd)
    {
        this._url = url;
        this._username = username;
        this._passwd = passwd;
    }

    /* TODO: better typing would be appericated */
    public void Login()
    {
        HttpClient c = new HttpClient { BaseAddress = new Uri(this._url) };
        var msg = new { this._username, this._passwd };
        var response = c.PostAsJsonAsync("/api/sessions/login", msg).Result
            .Content.ReadAsStringAsync().Result;
        Token =  JsonSerializer.Deserialize<JsonElement>(response).GetProperty("response").GetProperty("jwt").ToString();
    }

    public JsonElement SendMessage(int destinationId, string text)
    {
        HttpClient c = new HttpClient { BaseAddress = new Uri(this._url) };
        c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Token);
        MessageAddDto msg = new()
        {
            DestinationId = destinationId,
            Text = text,
        };
        var response = c.PostAsJsonAsync("/api/messages", msg).Result
            .Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<JsonElement>(response);
    }

    public JsonElement List()
    {
        HttpClient c = new HttpClient { BaseAddress = new Uri(this._url) };
        c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Token);
        var response = c.GetFromJsonAsync<JsonElement>("/api/group-chats/all").Result;
        return response;
    }
}
