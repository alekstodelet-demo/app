using Application.Repositories;

namespace Application.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IServiceRepository ServiceRepository { get; }
        IApplicationObjectRepository ApplicationObjectRepository { get; }
        IApplicationRepository ApplicationRepository { get; }
        IArchObjectRepository ArchObjectRepository { get; }
        void Commit();
        void Rollback();
    }
}
