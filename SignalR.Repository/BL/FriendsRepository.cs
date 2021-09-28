using SignalR.Model.DbModel;
using SignalR.Repository.Common;
using SignalR.Repository.Core;
using SignalR.Repository.DL;

namespace SignalR.Repository.BL
{
    public class FriendsRepository : GenericRepository<FriendModel>, IFriendsRepository
    {
        public FriendsRepository(SignalRContext context) : base(context)
        {
        }
    }
}
