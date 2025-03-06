using System.Data;
using Application.Repositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbConnection _mariadbConnection;
        private readonly IServiceProvider _serviceProvider;
        private IDbTransaction _dbTransaction;
        private IS_DocumentTemplateTranslationRepository? _S_DocumentTemplateTranslationRepository;
        private IS_DocumentTemplateRepository? _S_DocumentTemplateRepository;
        private IS_TemplateDocumentPlaceholderRepository? _S_TemplateDocumentPlaceholderRepository;
        private IS_DocumentTemplateTypeRepository? _S_DocumentTemplateTypeRepository;
        private IS_PlaceHolderTemplateRepository? _S_PlaceHolderTemplateRepository;
        private ILanguageRepository? _LanguageRepository;
        private IS_QueriesDocumentTemplateRepository? _S_QueriesDocumentTemplateRepository;
        private IS_PlaceHolderTypeRepository? _S_PlaceHolderTypeRepository;
        private IS_QueryRepository? _S_QueryRepository;
        private IHostEnvironment _appEnvironment;
        private ILogger<UnitOfWork> _logger;

        public UnitOfWork(DapperDbContext context, MariaDbContext mariaDbcontext, IHostEnvironment appEnvironment, 
            ILogger<UnitOfWork> logger, IServiceProvider serviceProvider)
        {
            _dbConnection = context.CreateConnection();
            _mariadbConnection = mariaDbcontext.CreateConnection();
            _dbConnection.Open();
            _dbTransaction = _dbConnection.BeginTransaction();
            _appEnvironment = appEnvironment;
            _serviceProvider = serviceProvider;
        }

        public IS_DocumentTemplateTranslationRepository S_DocumentTemplateTranslationRepository
        {
            get
            {
                if (_S_DocumentTemplateTranslationRepository == null)
                {
                    _S_DocumentTemplateTranslationRepository = new S_DocumentTemplateTranslationRepository(_dbConnection);
                    _S_DocumentTemplateTranslationRepository.SetTransaction(_dbTransaction);
                }
                return _S_DocumentTemplateTranslationRepository;
            }
        }
        public IS_DocumentTemplateRepository S_DocumentTemplateRepository
        {
            get
            {
                if (_S_DocumentTemplateRepository == null)
                {
                    _S_DocumentTemplateRepository = new S_DocumentTemplateRepository(_dbConnection);
                    _S_DocumentTemplateRepository.SetTransaction(_dbTransaction);
                }
                return _S_DocumentTemplateRepository;
            }
        }
        public IS_TemplateDocumentPlaceholderRepository S_TemplateDocumentPlaceholderRepository
        {
            get
            {
                if (_S_TemplateDocumentPlaceholderRepository == null)
                {
                    _S_TemplateDocumentPlaceholderRepository = new S_TemplateDocumentPlaceholderRepository(_dbConnection);
                    _S_TemplateDocumentPlaceholderRepository.SetTransaction(_dbTransaction);
                }
                return _S_TemplateDocumentPlaceholderRepository;
            }
        }
        public IS_DocumentTemplateTypeRepository S_DocumentTemplateTypeRepository
        {
            get
            {
                if (_S_DocumentTemplateTypeRepository == null)
                {
                    _S_DocumentTemplateTypeRepository = new S_DocumentTemplateTypeRepository(_dbConnection);
                    _S_DocumentTemplateTypeRepository.SetTransaction(_dbTransaction);
                }
                return _S_DocumentTemplateTypeRepository;
            }
        }
        public IS_PlaceHolderTemplateRepository S_PlaceHolderTemplateRepository
        {
            get
            {
                if (_S_PlaceHolderTemplateRepository == null)
                {
                    _S_PlaceHolderTemplateRepository = new S_PlaceHolderTemplateRepository(_dbConnection);
                    _S_PlaceHolderTemplateRepository.SetTransaction(_dbTransaction);
                }
                return _S_PlaceHolderTemplateRepository;
            }
        }
        public ILanguageRepository LanguageRepository
        {
            get
            {
                if (_LanguageRepository == null)
                {
                    _LanguageRepository = new LanguageRepository(_dbConnection);
                    _LanguageRepository.SetTransaction(_dbTransaction);
                }
                return _LanguageRepository;
            }
        }
        public IS_QueriesDocumentTemplateRepository S_QueriesDocumentTemplateRepository
        {
            get
            {
                if (_S_QueriesDocumentTemplateRepository == null)
                {
                    _S_QueriesDocumentTemplateRepository = new S_QueriesDocumentTemplateRepository(_dbConnection);
                    _S_QueriesDocumentTemplateRepository.SetTransaction(_dbTransaction);
                }
                return _S_QueriesDocumentTemplateRepository;
            }
        }
        public IS_PlaceHolderTypeRepository S_PlaceHolderTypeRepository
        {
            get
            {
                if (_S_PlaceHolderTypeRepository == null)
                {
                    _S_PlaceHolderTypeRepository = new S_PlaceHolderTypeRepository(_dbConnection);
                    _S_PlaceHolderTypeRepository.SetTransaction(_dbTransaction);
                }
                return _S_PlaceHolderTypeRepository;
            }
        }
        public IS_QueryRepository S_QueryRepository
        {
            get
            {
                if (_S_QueryRepository == null)
                {
                    _S_QueryRepository = _serviceProvider.GetRequiredService<IS_QueryRepository>();
                    // _S_QueryRepository = new S_QueryRepository(_dbConnection, _userRepository);
                    _S_QueryRepository.SetTransaction(_dbTransaction);
                }
                return _S_QueryRepository;
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
