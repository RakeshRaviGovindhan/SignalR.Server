using SignalR.Model.DbModel;
using System;
using System.Collections.Generic;

namespace SignalR.Service.Core
{
    public interface IConversationService
    {
        IEnumerable<ConversationModel> GetAllConversationsByUserId(Guid userId);
        ConversationModel GetConversationByUsersId(Guid firstUser, Guid secondUser);

        Guid AddOrUpdateConversation(Guid firstUser, Guid secondUser);
        void AddReply(string message, Guid conversationId, Guid userID);
    }
}
