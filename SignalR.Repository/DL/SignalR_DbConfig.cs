using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Repository.DL
{
    public class SignalR_DbConfig
    {
        public string DataBase_Name { get; set; }
        public string Connection_String { get; set; }

        public string User_Collection_Name { get; set; }
        public string Friend_Collection_Name { get; set; }
        public string Conversation_Collection_Name { get; set; }
        public string ConversationReply_Collection_Name { get; set; }
        public string Connection_Collection_Name { get; set; }
        public string RefreshToken_Collection_Name { get; set; }

    }
}
