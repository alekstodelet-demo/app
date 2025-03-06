using Application.Repositories;

namespace Application.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IS_PlaceHolderTemplateRepository S_PlaceHolderTemplateRepository { get; }
        IS_DocumentTemplateTranslationRepository S_DocumentTemplateTranslationRepository { get; }
        IS_DocumentTemplateRepository S_DocumentTemplateRepository { get; }
        IS_TemplateDocumentPlaceholderRepository S_TemplateDocumentPlaceholderRepository { get; }
        IS_DocumentTemplateTypeRepository S_DocumentTemplateTypeRepository { get; }
        ILanguageRepository LanguageRepository { get; }
        IS_QueriesDocumentTemplateRepository S_QueriesDocumentTemplateRepository { get; }
        IS_PlaceHolderTypeRepository S_PlaceHolderTypeRepository { get; }
        IS_QueryRepository S_QueryRepository { get; }
        void Commit();
        void Rollback();
    }
}
