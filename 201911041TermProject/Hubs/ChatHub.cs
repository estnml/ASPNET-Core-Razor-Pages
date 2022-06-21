using _201911041TermProject.Data;
using _201911041TermProject.Models;
using Microsoft.AspNetCore.SignalR;

namespace _201911041TermProject.Hubs
{


    public class ChatHub : Hub
    {


        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }
        public async Task SendMessage(string user, string message)
        {
            //message send to all users
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToGroup(string sender, string receiver, string message)
        {
            //message send to receiver only

            var newMessage = new Message()
            {
                Content = message,
                Date = DateTime.Now,
                SenderId = sender,
                ReceiverId = receiver
            };

            _context.Messages.Add(newMessage);
            _context.SaveChanges();

            return Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
        }
    }

}
