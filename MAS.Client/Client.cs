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

    /**
     *  <summary>
     *  Adds a user to group with given group id and user id
     *  </summary>
     */
    public JsonElement Invite(int gpid, int uid)
    {
        var response = c.PostAsync($"/api/group-chats/{gpid}/members/{uid}/add", null).Result
            .Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<JsonElement>(response);
    }
    /**
     * <summary>
     * Ban a user with given group id and user id
     * </summary>
     */
    public JsonElement Ban(int gpid, int uid)
    {
        var response = c.PutAsync($"/api/group-chats/{gpid}/members/{uid}/ban", null).Result
            .Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<JsonElement>(response);
    }
    /**
     * <summary>
     * Get name of users as a json list
     * </summary>
     */

    public JsonElement Names(int gpid)
    {
        var response = c.GetFromJsonAsync<JsonElement>($"/api/group-chats/{gpid}/members/").Result;
        return response;
    }
    /**
     * <summary>
     * Leaves group with the id
     * </summary>
     */
    public void Leave(int gpid)
    {
        var response = c.DeleteAsync($"/api/group-chats/{gpid}/members/").Result;
        // return response;
    }

    /* User */
    /**
     * <summary>
     * get list of users registered in the system as a json list
     * </summary>
     */
    public JsonElement Lusers() /* list users, taken from IRC */
    {

        var response = c.GetFromJsonAsync<JsonElement>($"/api/users/all").Result;
        return response;
        
    }
}
