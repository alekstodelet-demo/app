using System.Data;
using Application.Repositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _dbConnection;
        private readonly IServiceProvider _serviceProvider;
        private IDbTransaction _dbTransaction;
        private IServiceRepository? _serviceRepository;
        private ICustomerRepository? _customerRepository;
        private IContactTypeRepository? _contactTypeRepository;
        private ICustomerContactRepository? _customerContactRepository;
        private IApplicationObjectRepository? _applicationObjectRepository;
        private IApplicationRepository? _applicationRepository;
        private IArchObjectRepository? _archObjectRepository;
        private IUserRepository? _userRepository;
        private IHostEnvironment _appEnvironment;
        private IConfiguration _configuration;
        private ICustomerRequisiteRepository? _CustomerRequisiteRepository;
        private IOrganizationTypeRepository? _OrganizationTypeRepository;
        private IRepresentativeRepository? _RepresentativeRepository;
        private IRepresentativeContactRepository? _RepresentativeContactRepository;
        private IRepresentativeTypeRepository? _RepresentativeTypeRepository;


        public UnitOfWork(DapperDbContext context, IHostEnvironment appEnvironment, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _dbConnection = context.CreateConnection();
            _dbConnection.Open();
            _dbTransaction = _dbConnection.BeginTransaction();
            _appEnvironment = appEnvironment;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public IServiceRepository ServiceRepository
        {
            get
            {
                if (_serviceRepository == null)
                {
                    _serviceRepository = new ServiceRepository(_dbConnection, _configuration);
                    _serviceRepository.SetTransaction(_dbTransaction);
                }
                return _serviceRepository;
            }
        }


        public ICustomerRepository CustomerRepository
        {
            get
            {
                if (_customerRepository == null)
                {
                    _customerRepository = new CustomerRepository(_dbConnection);
                    _customerRepository.SetTransaction(_dbTransaction);
                }
                return _customerRepository;
            }
        }
        
        public IContactTypeRepository ContactTypeRepository
        {
            get
            {
                if (_contactTypeRepository == null)
                {
                    _contactTypeRepository = new ContactTypeRepository(_dbConnection, _configuration);
                    _contactTypeRepository.SetTransaction(_dbTransaction);
                }
                return _contactTypeRepository;
            }
        }

        public ICustomerContactRepository CustomerContactRepository
        {
            get
            {
                if (_customerContactRepository == null)
                {
                    _customerContactRepository = new CustomerContactRepository(_dbConnection);
                    _customerContactRepository.SetTransaction(_dbTransaction);
                }
                return _customerContactRepository;
            }
        }

        public IApplicationObjectRepository ApplicationObjectRepository
        {
            get
            {
                if (_applicationObjectRepository == null)
                {
                    _applicationObjectRepository = new ApplicationObjectRepository(_dbConnection);
                    _applicationObjectRepository.SetTransaction(_dbTransaction);
                }
                return _applicationObjectRepository;
            }
        }
        
        public IApplicationRepository ApplicationRepository
        {
            get
            {
                if (_applicationRepository == null)
                {
                    _applicationRepository = new ApplicationRepository(_dbConnection);
                    _applicationRepository.SetTransaction(_dbTransaction);
                }
                return _applicationRepository;
            }
        }
        
        public IArchObjectRepository ArchObjectRepository
        {
            get
            {
                if (_archObjectRepository == null)
                {
                    _archObjectRepository = new ArchObjectRepository(_dbConnection);
                    _archObjectRepository.SetTransaction(_dbTransaction);
                }
                return _archObjectRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_dbConnection, _configuration);
                    _userRepository.SetTransaction(_dbTransaction);
                }
                return _userRepository;
            }
        }


        public ICustomerRequisiteRepository CustomerRequisiteRepository
        {
            get
            {
                if (_CustomerRequisiteRepository == null)
                {
                    _CustomerRequisiteRepository = new CustomerRequisiteRepository(_dbConnection);
                    _CustomerRequisiteRepository.SetTransaction(_dbTransaction);
                }
                return _CustomerRequisiteRepository;
            }
        }
        public IOrganizationTypeRepository OrganizationTypeRepository
        {
            get
            {
                if (_OrganizationTypeRepository == null)
                {
                    _OrganizationTypeRepository = new OrganizationTypeRepository(_dbConnection);
                    _OrganizationTypeRepository.SetTransaction(_dbTransaction);
                }
                return _OrganizationTypeRepository;
            }
        }
        public IRepresentativeRepository RepresentativeRepository
        {
            get
            {
                if (_RepresentativeRepository == null)
                {
                    _RepresentativeRepository = new RepresentativeRepository(_dbConnection);
                    _RepresentativeRepository.SetTransaction(_dbTransaction);
                }
                return _RepresentativeRepository;
            }
        }
        public IRepresentativeContactRepository RepresentativeContactRepository
        {
            get
            {
                if (_RepresentativeContactRepository == null)
                {
                    _RepresentativeContactRepository = new RepresentativeContactRepository(_dbConnection);
                    _RepresentativeContactRepository.SetTransaction(_dbTransaction);
                }
                return _RepresentativeContactRepository;
            }
        }
        public IRepresentativeTypeRepository RepresentativeTypeRepository
        {
            get
            {
                if (_RepresentativeTypeRepository == null)
                {
                    _RepresentativeTypeRepository = new RepresentativeTypeRepository(_dbConnection);
                    _RepresentativeTypeRepository.SetTransaction(_dbTransaction);
                }
                return _RepresentativeTypeRepository;
            }
        }


        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
            }
            catch
            {
                _dbTransaction.Rollback();
                throw;
            }
            finally
            {
                _dbTransaction.Dispose();
                _dbTransaction = _dbConnection.BeginTransaction();
            }
        }

        public void Rollback()
        {
            _dbTransaction.Rollback();
            _dbTransaction.Dispose();
            _dbTransaction = _dbConnection.BeginTransaction();
        }

        public void Dispose()
        {
            _dbTransaction?.Dispose();
            _dbConnection?.Dispose();
        }
    }
}
