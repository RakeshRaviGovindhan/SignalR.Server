using SignalR.Repository.Core;

namespace SignalR.Service.Base
{
    public class BaseService
    {
        private readonly IUnitOfWork unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        protected IUnitOfWork UnitOfWork => unitOfWork;
    }
}
