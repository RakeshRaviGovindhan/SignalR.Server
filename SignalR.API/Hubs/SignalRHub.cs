using Microsoft.AspNetCore.SignalR;
using SignalR.Model.DbModel;
using SignalR.Service.Core;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.API.Hubs
{
    public class SignalRHub : Hub
    {
        private readonly IUserService userService;
        private readonly IConversationService conversationService;

        public SignalRHub(IUserService userService, IConversationService conversationService)
        {
            this.userService = userService;
            this.conversationService = conversationService;
        }

        public async Task SendMessage(string userId, string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", userId, message);
        }

        public async Task SendPrivateMessage(string userEmail, string message)
        {

            var senderUser = userService.GetUserByConnectionId(Context.ConnectionId);
            var friend = userService.GetUserByEmail(userEmail);
            var friendConnections = friend.Connections.Where(x => x.IsConnected);
            foreach (var connection in friendConnections)
            {
                await Clients.Client(connection.ConnectionID).SendAsync("ReceivePrivateMessage", userEmail, message);
            }
            // Inser in to database..
            var conversationModel = conversationService.GetConversationByUsersId(senderUser.Id, friend.Id);

            if (conversationModel == null)
            {
                var conversationId = conversationService.AddOrUpdateConversation(senderUser.Id, friend.Id);
                conversationService.AddReply(message, conversationId, senderUser.Id);
            }
            else
            {
                conversationService.AddReply(message, conversationModel.Id, senderUser.Id);
            }
        }

        public async Task CallFriendAsync(string userEmail)
        {
            var senderUser = userService.GetUserByConnectionId(Context.ConnectionId);
            var friend = userService.GetUserByEmail(userEmail);
            var friendConnections = friend.Connections.Where(x => x.IsConnected && x.IsAvailable);

            foreach (var connection in friendConnections)
            {
                await Clients.Client(connection.ConnectionID).SendAsync("ReceivePrivateVideoCall", senderUser.Email);
            }
        }

        public async Task AcceptVideoCall(string currentUser, string friendUser)
        {
            var senderUser = userService.GetUserByConnectionId(Context.ConnectionId);
            var friend = userService.GetUserByEmail(friendUser);
            var friendConnections = friend.Connections.Where(x => x.IsConnected);
            foreach (var connection in friendConnections)
            {
                await Clients.Client(connection.ConnectionID).SendAsync("AcceptVideoCallByFriend", currentUser, friendUser);
            }
        }

        public async Task RejectVideoCall(string currentUser, string friendUser)
        {
            var senderUser = userService.GetUserByConnectionId(Context.ConnectionId);
            var friend = userService.GetUserByEmail(friendUser);
            var friendConnections = friend.Connections.Where(x => x.IsConnected);
            foreach (var connection in friendConnections)
            {
                await Clients.Client(connection.ConnectionID).SendAsync("RejectVideoCallByFriend", currentUser, friendUser);
            }
        }



        public async Task OnConnect(string userEmail)
        {
            var user = userService.GetUserByEmail(userEmail);
            userService.AddUserConnections(new ConnectionModel
            {
                ConnectionID = Context.ConnectionId,
                IsConnected = true,
                UserAgent = Context.GetHttpContext().Request.Headers["User-Agent"],
                UserID = user.Id
            });

            await base.OnConnectedAsync();
        }

        public async Task OnDisconnect(string userEmail)
        {
            var user = userService.GetUserByEmail(userEmail);
            userService.UpdateUserConnectionsStatus(user.Id, false, Context.ConnectionId);
            await base.OnDisconnectedAsync(null);
        }
    }
}
