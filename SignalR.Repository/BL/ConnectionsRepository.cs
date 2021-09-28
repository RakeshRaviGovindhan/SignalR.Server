using SignalR.Model.DbModel;
using SignalR.Repository.Common;
using SignalR.Repository.Core;
using SignalR.Repository.DL;

namespace SignalR.Repository.BL
{
    public class ConnectionsRepository : GenericRepository<ConnectionModel>, IConnectionsRepository
    {
        public ConnectionsRepository(SignalRContext context) 
            : base(context)
        {
        }
    }
}
