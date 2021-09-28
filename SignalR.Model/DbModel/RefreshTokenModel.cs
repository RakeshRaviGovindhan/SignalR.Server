using SignalR.Model.Base;

namespace SignalR.Model.DbModel
{
    public class RefreshTokenModel : BaseModel
    {
        public string UserEmail { get; set; }
        public string RefreshToken { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
