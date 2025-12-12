using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace MASsenger.Application.Hubs
{
    /* Taken from https://github.com/bliny-so-smetanoi/messenger-app/
     * 
     * Permission to use, copy, modify, and/or distribute this software for any purpose with or without fee is hereby granted,
     * provided that the above copyright notice and this permission notice appear in all copies
     */
    public class ChatHub : Hub
    {
        private readonly IMemoryCache _memoryCache;

        public ChatHub(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public override async Task OnConnectedAsync()
        {
            var contextHttp = Context.GetHttpContext();
            var userId = contextHttp.Request.ToString();
            var connectionId = Context.ConnectionId;

            string space = null;

            if (!_memoryCache.TryGetValue(userId, out space))
            {
                _memoryCache.Set(userId, connectionId);
            }
            else
            {
                _memoryCache.Remove(userId);
                _memoryCache.Set(userId, connectionId);
            }

            Console.WriteLine("After adding:" + _memoryCache.Get(userId));

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.GetHttpContext().Request.Query["token"].ToString();
            Console.WriteLine("Before delete:" + _memoryCache.Get(userId));
            _memoryCache.Remove(userId);

            Console.WriteLine("After delete: " + _memoryCache.Get(userId));

            await base.OnDisconnectedAsync(exception);
        }
    }
}
