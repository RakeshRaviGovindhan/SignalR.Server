using SignalR.Model.DbModel;
using SignalR.Repository.Common;
using SignalR.Repository.Core;
using SignalR.Repository.DL;

namespace SignalR.Repository.BL
{
    public class ConversationRepliesRepository : GenericRepository<ConversationReplyModel>, IConversationRepliesRepository
    {
        public ConversationRepliesRepository(SignalRContext context) : base(context)
        {
        }
    }
}
