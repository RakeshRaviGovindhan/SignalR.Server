using SignalR.Model.Base;
using System;

namespace SignalR.Model.DbModel
{
    public class FriendModel : BaseModel
    {
        public Guid UserID { get; set; }
        public Guid UserFriendId { get; set; }
        public UserModel UserFriend { get; set; }
    }
}
