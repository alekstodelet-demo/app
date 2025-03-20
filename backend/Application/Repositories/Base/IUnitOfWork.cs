using Application.Repositories;

namespace Application.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IServiceRepository ServiceRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        ICustomerContactRepository CustomerContactRepository { get; }
        IApplicationObjectRepository ApplicationObjectRepository { get; }
        IApplicationRepository ApplicationRepository { get; }
        IArchObjectRepository ArchObjectRepository { get; }
        IUserRepository UserRepository { get; }
        void Commit();
        void Rollback();
    }
}
