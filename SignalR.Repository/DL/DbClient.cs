using MongoDB.Driver;
using SignalR.Model.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Repository.DL
{
    public class DbClient : IDbClient
    {
        #region Properties

        private readonly IMongoCollection<UserModel> UserCollection;
        private readonly IMongoCollection<FriendModel> FriendCollection;
        private readonly IMongoCollection<ConversationModel> ConversationCollection;
        private readonly IMongoCollection<ConversationReplyModel> ConversationReplyCollection;
        private readonly IMongoCollection<ConnectionModel> ConnectionCollection;
        private readonly IMongoCollection<RefreshTokenModel> RefreshTokenCollection;

        #endregion

        #region Methods

        public IMongoCollection<ConnectionModel> GetConnectionCollection() => ConnectionCollection;

        public IMongoCollection<ConversationModel> GetConversationCollection() => ConversationCollection;

        public IMongoCollection<ConversationReplyModel> GetConversationReplyCollection() => ConversationReplyCollection;

        public IMongoCollection<FriendModel> GetFriendCollection() => FriendCollection;

        public IMongoCollection<RefreshTokenModel> GetRefreshTokenCollection() => RefreshTokenCollection;

        public IMongoCollection<UserModel> GetUserCollection() => UserCollection;

        #endregion

    }
}
