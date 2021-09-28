using Microsoft.AspNetCore.Http;
using SignalR.Model.DbModel;
using System;
using System.Collections.Generic;

namespace SignalR.Service.Core
{
    public interface IUserService
    {
        UserModel GetUserById(Guid userId);
        UserModel GetUserByEmail(string email);
        void AddUserConnections(ConnectionModel conversationModel);
        void UpdateUserConnectionsStatus(Guid userId, bool status, string connectionID);
        UserModel Login(LoginModel loginModel, HttpContext httpContext);
        UserModel RefreshToken(string refreshToken, HttpContext httpContext);
        Guid InsertUser(UserModel userModel);
        IEnumerable<UserModel> GetMyFriends(Guid userID);
        UserModel GetUserByConnectionId(string connectionId);
    }
}
