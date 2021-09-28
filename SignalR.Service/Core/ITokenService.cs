using SignalR.Model.DbModel;

namespace SignalR.Service.Core
{
    public interface ITokenService
    {
        RefreshTokenModel AddToken(string userEmail, string ipAddress, string userAgent);
        RefreshTokenModel GetRefreshToken(string refreshToken, string userAgent);
        RefreshTokenModel GetRefreshTokenByEmail(string email, string userAgent);
        RefreshTokenModel UpdateToken(string refreshToekn, string userAgent);
    }
}
