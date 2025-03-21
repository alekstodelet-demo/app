using Application.Repositories;

namespace Application.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IServiceRepository ServiceRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        ICustomerContactRepository CustomerContactRepository { get; }
        IContactTypeRepository ContactTypeRepository { get; }
        IApplicationObjectRepository ApplicationObjectRepository { get; }
        IApplicationRepository ApplicationRepository { get; }
        IArchObjectRepository ArchObjectRepository { get; }
        IUserRepository UserRepository { get; }
        ICustomerRequisiteRepository CustomerRequisiteRepository { get; }
        IOrganizationTypeRepository OrganizationTypeRepository { get; }
        IRepresentativeRepository RepresentativeRepository { get; }
        IRepresentativeContactRepository RepresentativeContactRepository { get; }
        IRepresentativeTypeRepository RepresentativeTypeRepository { get; }


        void Commit();
        void Rollback();
    }
}
