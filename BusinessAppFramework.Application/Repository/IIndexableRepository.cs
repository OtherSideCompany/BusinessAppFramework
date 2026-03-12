using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Repository
{
    public interface IIndexableRepository
    {
        Task<int> GetNewIndexAsync();
        Task<int> GetNewYearIndexAsync(int year);
        Task<int> GetNewIndexInParentAsync(int parentId, string relationKey);
    }
}
