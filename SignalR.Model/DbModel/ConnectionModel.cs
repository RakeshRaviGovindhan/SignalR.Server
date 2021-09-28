using SignalR.Model.Base;
using System;

namespace SignalR.Model.DbModel
{
    public class ConnectionModel : BaseModel
    {
        public string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public bool IsConnected { get; set; }
        public bool IsAvailable { get; set; } = true;
        public Guid UserID { get; set; }
        public UserModel User { get; set; }

    }
}
