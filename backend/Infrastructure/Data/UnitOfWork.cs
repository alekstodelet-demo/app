using System.Data;
using Application.Repositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApi.Infrastructure;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _dbConnection;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UnitOfWork> _logger;
        private readonly IRepositoryFactory _repositoryFactory;
        private IDbTransaction _dbTransaction;
        private IServiceRepository? _serviceRepository;
        private IApplicationObjectRepository? _applicationObjectRepository;
        private IApplicationRepository? _applicationRepository;
        private IArchObjectRepository? _archObjectRepository;
        private IUserRepository? _userRepository;
        private IHostEnvironment _appEnvironment;
        private IConfiguration _configuration;
        
        public UnitOfWork(DapperDbContext context, IHostEnvironment appEnvironment, IServiceProvider serviceProvider, 
            IConfiguration configuration, ILogger<UnitOfWork> logger, IRepositoryFactory repositoryFactory)
        {
            _dbConnection = context.CreateConnection();
            _dbConnection.Open();
            _dbTransaction = _dbConnection.BeginTransaction();
            _appEnvironment = appEnvironment;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _repositoryFactory = repositoryFactory;
            _logger = logger;
        }

        public IServiceRepository ServiceRepository
        {
            get
            {
                if (_serviceRepository == null)
                {
                    _serviceRepository = _repositoryFactory.CreateServiceRepository();
                    
                    // Если репозиторий поддерживает транзакции базы данных, устанавливаем транзакцию
                    if (_serviceRepository is Infrastructure.Repositories.ServiceRepository dbRepository)
                    {
                        dbRepository.SetTransaction(_dbTransaction);
                    }
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

        /// <summary>
        /// Подтверждает все изменения, сделанные в контексте транзакции
        /// </summary>
        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
                _logger.LogInformation("Transaction committed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error committing transaction");
                _dbTransaction.Rollback();
                throw;
            }
            finally
            {
                _dbTransaction.Dispose();
                _dbTransaction = _dbConnection.BeginTransaction();
                ResetRepositories();
            }
        }

        /// <summary>
        /// Откатывает все изменения, сделанные в контексте транзакции
        /// </summary>
        public void Rollback()
        {
            try
            {
                _dbTransaction.Rollback();
                _logger.LogInformation("Transaction rolled back successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rolling back transaction");
                throw;
            }
            finally
            {
                _dbTransaction.Dispose();
                _dbTransaction = _dbConnection.BeginTransaction();
                ResetRepositories();
            }
        }
        
        /// <summary>
        /// Сбрасывает кэшированные репозитории
        /// </summary>
        private void ResetRepositories()
        {
            _serviceRepository = null;
            _applicationObjectRepository = null;
            _applicationRepository = null;
            _archObjectRepository = null;
            _userRepository = null;
        }

        public void Dispose()
        {
            _dbTransaction?.Dispose();
            _dbConnection?.Dispose();
        }
    }
}
