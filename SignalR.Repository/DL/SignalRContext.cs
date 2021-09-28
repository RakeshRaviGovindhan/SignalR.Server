using Microsoft.EntityFrameworkCore;
using SignalR.Model.DbModel;

namespace SignalR.Repository.DL
{
    public class SignalRContext : DbContext
    {
        public SignalRContext(DbContextOptions<SignalRContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<FriendModel> Friends { get; set; }
        public DbSet<ConversationModel> Conversations { get; set; }
        public DbSet<ConversationReplyModel> ConversationReplies { get; set; }
        public DbSet<ConnectionModel> Connections { get; set; }
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
