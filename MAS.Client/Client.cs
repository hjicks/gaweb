using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using MAS.Application.Dtos.MessageDtos;

namespace MAS.Client;
public class Client
{
    private readonly string url;
    private readonly string username;
    private readonly string password;
    private readonly string clientName;
    private readonly string os;
    private HttpClient c;

    public string? /* JWT */ token;
    public string? /* JWT */ refreshToken;
    public Client(string url, string username, string passwd, string clientName, string os)
    {
        this.url = url;
        this.username = username;
        this.password = passwd;
        this.clientName = clientName;
        this.os = os;
        c = new HttpClient { BaseAddress = new Uri(this.url) };
    }

    /* Session */

    /**
     * <summary>
     * Logs user in with information given by the constructor
     * </summary>
     */
    public void Login()
    {
        c.DefaultRequestHeaders.Authorization = null;

        /* better typing would be appericated */
        var msg = new { this.username, this.password, this.clientName, this.os };
        string response = c.PostAsJsonAsync("/api/sessions/login", msg).Result
            .Content.ReadAsStringAsync().Result;

        JsonElement jsonresponse = JsonSerializer.Deserialize<JsonElement>(response).GetProperty("response");
        this.token =  jsonresponse.GetProperty("jwt").ToString();
        this.refreshToken = jsonresponse.GetProperty("refreshToken").ToString();
        c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.token);
    }

    /**
     * <summary>
     * Refreshes user's token
     * </summary>
     */
    public void Refresh()
    {
        c.DefaultRequestHeaders.Authorization = null;
        var msg = new { this.refreshToken };
        var response = c.PostAsJsonAsync("/api/sessions/refresh", msg).Result
            .Content.ReadAsStringAsync().Result;

        JsonElement jsonresponse = JsonSerializer.Deserialize<JsonElement>(response).GetProperty("response");
        this.token = jsonresponse.GetProperty("jwt").ToString();
        this.refreshToken = jsonresponse.GetProperty("refreshToken").ToString();
        c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.token);
    }


    
    /* Message */
    public JsonElement SendMessage(int destinationId, string text)
    {
        MessageAddDto msg = new()
        {
            DestinationId = destinationId,
            Text = text,
        };
        var response = c.PostAsJsonAsync("/api/messages", msg).Result
            .Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<JsonElement>(response);
    }

    /* GroupChat */
    /**
     *  <summary>
     *  Returns list of groups
     *  </summary>
     */
    public JsonElement List()
    {
        var response = c.GetFromJsonAsync<JsonElement>("/api/group-chats/all").Result;
        return response;
    }
}
