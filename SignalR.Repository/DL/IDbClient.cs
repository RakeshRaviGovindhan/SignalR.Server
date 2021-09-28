using MongoDB.Driver;
using SignalR.Model.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Repository.DL
{
    public interface IDbClient
    {
        IMongoCollection<UserModel> GetUserCollection();
        IMongoCollection<FriendModel> GetFriendCollection();
        IMongoCollection<ConversationModel> GetConversationCollection();
        IMongoCollection<ConversationReplyModel> GetConversationReplyCollection();
        IMongoCollection<ConnectionModel> GetConnectionCollection();
        IMongoCollection<RefreshTokenModel> GetRefreshTokenCollection();
    }

}
