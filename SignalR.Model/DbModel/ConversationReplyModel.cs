using SignalR.Model.Base;
using System;

namespace SignalR.Model.DbModel
{
    public class ConversationReplyModel : BaseModel
    {

        /// <summary>
        /// The sender user id.
        /// </summary>
        public Guid SenderUserId { get; set; }

        /// <summary>
        /// The conversation reply content
        /// </summary>
        public string Content { get; set; }

        public Guid ConversationID { get; set; }
        /// <summary>
        /// The  association of the conversation   
        /// </summary>
        //[ForeignKey("ConversationId")]
        public ConversationModel Conversation { get; set; }


    }
}
