using SignalR.Model.DbModel;
using SignalR.Repository.Common;
using SignalR.Repository.Core;
using SignalR.Repository.DL;

namespace ChatApp.Repositories
{
    public class TokensRepository : GenericRepository<RefreshTokenModel>, ITokensRepository
    {
        public TokensRepository(SignalRContext context)
            : base(context)
        {
        }
    }
}
