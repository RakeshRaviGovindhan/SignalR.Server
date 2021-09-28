using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SignalR.Model.DbModel;
using SignalR.Model.Settings;
using SignalR.Repository.Core;
using SignalR.Service.Base;
using SignalR.Service.Core;
using SignalR.Service.Extension;
using SignalR.Service.Tool;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SignalR.Service.BL
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUsersRepository usersRepository;
        private ITokenService tokenService;
        private readonly IFriendsRepository friendsRepository;
        private readonly IConnectionsRepository connectionsRepository;
        private readonly AppSettings appSettings;

        public UserService(IUnitOfWork unitOfWork,
            IUsersRepository usersRepository,
            ITokenService tokensManager,
            IFriendsRepository friendsRepository,
            IConnectionsRepository connectionsRepository,
            IOptions<AppSettings> appSettings)
            : base(unitOfWork)
        {
            this.usersRepository = usersRepository;
            this.tokenService = tokensManager;
            this.friendsRepository = friendsRepository;
            this.connectionsRepository = connectionsRepository;
            this.appSettings = appSettings.Value;
        }

        public Guid InsertUser(UserModel userModel)
        {
            userModel.Email = userModel.Email.ToLower();
            var user = GetUserByEmail(userModel.Email);
            if (user != null)
            {
                return Guid.Empty;
            }

            var now = DateTime.Now;
            var salt = PasswordHasher.GenerateSalt();
            var pwdHash = Convert.ToBase64String(PasswordHasher.ComputeHash(userModel.Password, salt));

            userModel.Date = now;
            userModel.ValidationToken = Guid.NewGuid().ToString();
            userModel.PasswordSalt = Convert.ToBase64String(salt);
            userModel.Password = pwdHash;

            var modelId = usersRepository.Insert(userModel);
            UnitOfWork.Commit();
            return modelId;
        }

        public UserModel GetUserById(Guid userId)
        {
            return usersRepository.Get(x => x.Id == userId, includeProperties: "Friends").SingleOrDefault().WithoutPassword();
        }

        public UserModel GetUserByEmail(string email)
        {
            return usersRepository.Get(x => x.Email == email, includeProperties: "Friends,Connections").SingleOrDefault().WithoutPassword();

        }

        public void AddUserConnections(ConnectionModel conversationModel)
        {
            connectionsRepository.Insert(conversationModel);
            UnitOfWork.Commit();
            //var dbUserModel = usersRepository.GetByID(userId);
            //dbUserModel.Connections.Add(conversationModel);
            //usersRepository.Update(dbUserModel);
            //UnitOfWork.Commit();
        }

        public void UpdateUserConnectionsStatus(Guid userId, bool status, string connectionID)
        {
            var connection = connectionsRepository.Get(x => x.ConnectionID == connectionID && x.UserID == userId).SingleOrDefault();
            if (connection != null)
            {
                connection.IsConnected = status;
                connectionsRepository.Update(connection);
                UnitOfWork.Commit();
            }
        }

        public UserModel Login(LoginModel loginModel, HttpContext httpContext)
        {
            double minutes = 2d;
            loginModel.Email = loginModel.Email.ToLower();
            DateTime now = DateTime.UtcNow;
            var ipAddress = httpContext.Connection.RemoteIpAddress;
            var userAgent = httpContext.Request.Headers["User-Agent"];

            var user = usersRepository.Get(x => x.Email == loginModel.Email && string.IsNullOrEmpty(x.ValidationToken)).SingleOrDefault();

            if (user != null)
            {
                bool verified = PasswordHasher.VerifyPassword(loginModel.Password, user.PasswordSalt, user.Password);
                if (verified)
                {
                    var refreshToken = tokenService.GetRefreshTokenByEmail(loginModel.Email, userAgent);
                    if (refreshToken != null && refreshToken.IpAddress != ipAddress.ToString())
                    {
                        // TODO Send notification to the user and the admin => May be  Hacker attck.
                    }

                    if (refreshToken != null)
                    {
                        refreshToken = tokenService.UpdateToken(refreshToken.RefreshToken, userAgent);
                    }
                    else
                    {
                        refreshToken = tokenService.AddToken(loginModel.Email, ipAddress.ToString(), userAgent);
                    }

                    string newToken = GenerateToken(userAgent, user, now, minutes);
                    user.RefreshToken = refreshToken.RefreshToken;
                    user.TokenExpireTimes = now.AddMinutes(minutes);
                    user.Token = newToken;
                    return user.WithoutPassword();
                }
                else
                {
                    // TODO BG: LOG login failed email; address ip and user agent. 
                }
            }
            return null;
        }

        public UserModel RefreshToken(string refreshToken, HttpContext httpContext)
        {
            DateTime now = DateTime.UtcNow;
            double minutes = 2d;
            var userAgent = httpContext.Request.Headers["User-Agent"];
            var ipAddress = httpContext.Connection.RemoteIpAddress;
            var refToken = tokenService.GetRefreshToken(refreshToken, userAgent);

            if (refToken == null)
            {
                //logger.LogWarning($"Probable hacker attack attempt for refresh token: {refreshToken}, IP : {ipAddress}");
                return null;
            }

            if (ipAddress.ToString() != refToken.IpAddress)
            {
                //logger.LogWarning($"Token not found token: {refreshToken} ");
                return null;
            }

            var user = GetUserByEmail(refToken.UserEmail).WithoutPassword();
            if (user == null)
            {
                //logger.LogWarning($" user not found for token: {refreshToken} ");
                return null;
            }

            string newToken = GenerateToken(userAgent, user, now, minutes);
            refToken = tokenService.UpdateToken(refToken.RefreshToken, userAgent);
            user.RefreshToken = refToken.RefreshToken;
            user.TokenExpireTimes = now.AddMinutes(minutes);
            user.Token = newToken;
            return user;
        }

        public IEnumerable<UserModel> GetMyFriends(Guid userID)
        {
            return friendsRepository.Get(x => x.UserID == userID, includeProperties: "UserFriend").Select(x => x.UserFriend);
        }

        private string GenerateToken(string userAgent, UserModel user, DateTime now, double minutes)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = appSettings.JwtIssuer,
                Audience = string.IsNullOrEmpty(userAgent) ? appSettings.JwtMobileAudience : appSettings.JwtWebAudience,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Id", user.Id.ToString()),
                }),
                Expires = now.AddMinutes(minutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public UserModel GetUserByConnectionId(string connectionId)
        {
            return connectionsRepository.Get(x => x.ConnectionID == connectionId, includeProperties: "User").SingleOrDefault()?.User;
        }
    }
}
