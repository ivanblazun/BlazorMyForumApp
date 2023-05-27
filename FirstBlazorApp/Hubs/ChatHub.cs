using Microsoft.AspNetCore.SignalR;

namespace FirstBlazorApp.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage( string user, string message)
        {
            await Clients.All.SendAsync("RecievedMessage",user,message);
        }
    }
}
