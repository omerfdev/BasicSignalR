using BasicSignalR.Db;
using BasicSignalR.Models;
using Microsoft.AspNetCore.SignalR;

namespace BasicSignalR.Hubs
{
    public class ChatHub:Hub
    {
        //public async Task SendMessage(string username,string message)
        //{
        //   await Clients.All.SendAsync("NewMessage",username, message);
        //}
        //public async Task SendLocation(string color,int x,int y)
        //{
        //    await Clients.All.SendAsync("GetLocation",Context.ConnectionId,color,x,y);
        //}
        private readonly AppDbContext _dbContext;

        private static Dictionary<string, string> connectedClients = new Dictionary<string, string>();

        public ChatHub(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

            // Save the message to the database
            await SaveMessageToDatabase(user, message);
        }

        public async Task JoinChat(string user, string message)
        {
            connectedClients[Context.ConnectionId] = user;
            await Clients.Others.SendAsync("ReceiveMessage", user, message);

            // Save the join message to the database
            await SaveMessageToDatabase(user, message);
        }

        private async Task LeaveChat()
        {
            if (connectedClients.TryGetValue(Context.ConnectionId, out string user))
            {
                var message = $"{user} left the chat";
                await Clients.Others.SendAsync("ReceiveMessage", user, message);

                // Save the leave message to the database
                await SaveMessageToDatabase(user, message);
            }
        }

        private async Task SaveMessageToDatabase(string user, string message)
        {
            // Get the receiver's ID from the connectedClients dictionary using Context.ConnectionId
            var receiverId = connectedClients.GetValueOrDefault(Context.ConnectionId);

            if (!string.IsNullOrEmpty(receiverId))
            {
                // Save the message to the database
                var conversation = new Conversation
                {
                    SenderId = user,
                    ReceiverId = receiverId,
                    Message = message,
                    Timestamp = DateTime.Now
                };

                _dbContext.Conversations.Add(conversation);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}   
