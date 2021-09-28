using SignalR.Model.DbModel;
using SignalR.Repository.Common;
using SignalR.Repository.Core;
using SignalR.Repository.DL;

namespace SignalR.Repository.BL
{
    public class UsersRepository : GenericRepository<UserModel>, IUsersRepository
    {
        public UsersRepository(SignalRContext context) : base(context)
        {
        }
    }
}
