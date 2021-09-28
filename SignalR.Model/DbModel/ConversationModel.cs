using SignalR.Model.Base;
using System;
using System.Collections.Generic;

namespace SignalR.Model.DbModel
{
    public class ConversationModel : BaseModel
    {
        /// <summary>
        /// The conversation replies.
        /// </summary>
        public ICollection<ConversationReplyModel> ConversationsReplies { get; set; }

        /// <summary>
        /// The first user id.
        /// </summary>
        public Guid UserOneID { get; set; }


        /// <summary>
        /// The second user id.
        /// </summary>
        public Guid UserTwoID { get; set; }



    }
}
