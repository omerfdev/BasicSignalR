using Microsoft.AspNetCore.SignalR;

namespace BasicSignalR.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(string username,string message)
        {
           await Clients.All.SendAsync("NewMessage",username, message);
        }
        public async Task SendLocation(string color,int x,int y)
        {
            await Clients.All.SendAsync("GetLocation",Context.ConnectionId,color,x,y);
        }
    }
}   
