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
        private IApplicationObjectRepository? _applicationObjectRepository;
        private IApplicationRepository? _applicationRepository;
        private IArchObjectRepository? _archObjectRepository;
        private IHostEnvironment _appEnvironment;
        private IConfiguration _configuration;
        
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
