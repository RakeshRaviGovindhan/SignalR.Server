using SignalR.Model.DbModel;
using SignalR.Repository.Core;
using SignalR.Service.Base;
using SignalR.Service.Core;
using System;
using System.Linq;

namespace SignalR.Service.BL
{
    public class TokenService : ITokenService
    {
        private readonly ITokensRepository tokensRepository;

        public TokenService(ITokensRepository tokensRepository)
        {
            this.tokensRepository = tokensRepository;
        }

        public RefreshTokenModel GetRefreshToken(string refreshToken, string userAgent)
        {
            string incomeSource = string.IsNullOrEmpty(userAgent) ? "Mobile" : "Web";

            return tokensRepository.Get(x => x.RefreshToken == refreshToken && x.UserAgent == incomeSource).SingleOrDefault();
        }

        public RefreshTokenModel GetRefreshTokenByEmail(string email, string userAgent)
        {
            string incomeSource = string.IsNullOrEmpty(userAgent) ? "Mobile" : "Web";
            return tokensRepository.Get(x => x.UserEmail == email && x.UserAgent == incomeSource).SingleOrDefault();
        }

        public RefreshTokenModel UpdateToken(string refreshToekn, string userAgent)
        {
            var tokenModel = GetRefreshToken(refreshToekn, userAgent);
            if (tokenModel == null)
            {
                return null;
            }

            tokenModel.RefreshToken = Guid.NewGuid().ToString();
            tokenModel.Date = DateTime.Now;
            tokensRepository.Update(tokenModel);
            UnitOfWork.Commit();
            return tokenModel;
        }

        public RefreshTokenModel AddToken(string userEmail, string ipAddress, string userAgent)
        {
            var now = DateTime.Now;
            RefreshTokenModel tokenModel = new RefreshTokenModel
            {
                UserEmail = userEmail,
                RefreshToken = Guid.NewGuid().ToString(),
                Date = now,
                IpAddress = ipAddress,
                UserAgent = string.IsNullOrEmpty(userAgent) ? "Mobile" : "Web"
            };
            tokensRepository.Insert(tokenModel);
            UnitOfWork.Commit();

            return tokenModel;
        }
    }
}
