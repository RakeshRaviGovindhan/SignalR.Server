using SignalR.Model.DbModel;
using SignalR.Repository.Core;
using SignalR.Service.Base;
using SignalR.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalR.Service.BL
{
    public class ConversationService : BaseService, IConversationService
    {
        private readonly IConversationsRepository conversationsRepository;
        private readonly IConversationRepliesRepository conversationRepliesRepository;

        public ConversationService(IUnitOfWork unitOfWork,
            IConversationsRepository conversationsRepository,
            IConversationRepliesRepository conversationRepliesRepository)
            : base(unitOfWork)
        {
            this.conversationsRepository = conversationsRepository;
            this.conversationRepliesRepository = conversationRepliesRepository;
        }

        public IEnumerable<ConversationModel> GetAllConversationsByUserId(Guid userId)
        {
            return conversationsRepository.Get(x => x.UserOneID == userId || x.UserTwoID == userId, includeProperties: "ConversationReplies");
        }

        public ConversationModel GetConversationByUsersId(Guid firstUser, Guid secondUser)
        {
            return conversationsRepository.Get(x => (x.UserOneID == firstUser && x.UserTwoID == secondUser) || (x.UserOneID == secondUser && x.UserTwoID == firstUser), includeProperties: "ConversationsReplies").SingleOrDefault();
        }
        public Guid AddOrUpdateConversation(Guid firstUser, Guid secondUser)
        {
            var now = DateTime.UtcNow;
            var conversation = GetConversationByUsersId(firstUser, secondUser);
            if (conversation == null)
            {
                conversation = new ConversationModel
                {
                    UserOneID = firstUser,
                    UserTwoID = secondUser,
                    Date = now
                };

               conversationsRepository.Insert(conversation);
             
            }
            else
            {
                conversation.Date = DateTime.Now;
                conversationsRepository.Update(conversation);
            }

            UnitOfWork.Commit();
            return conversation.Id;
        }
        public void AddReply(string message, Guid conversationId, Guid userID)
        {
            conversationRepliesRepository.Insert(new ConversationReplyModel
            {
                Content = message,
                ConversationID = conversationId,
                Date = DateTime.Now,
                SenderUserId = userID
            });
            UnitOfWork.Commit();
        }

    }
}
