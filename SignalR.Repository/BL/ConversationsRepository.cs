using SignalR.Model.DbModel;
using SignalR.Repository.Common;
using SignalR.Repository.Core;
using SignalR.Repository.DL;

namespace SignalR.Repository.BL
{
    public class ConversationsRepository : GenericRepository<ConversationModel>, IConversationsRepository
    {
        public ConversationsRepository(SignalRContext context) : base(context)
        {
        }
    }
}
