using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Search
{
    public interface IDomainObjectTreeSearch : IDisposable
    {
        Task SearchAsync(DomainObject parent);
        void Dispose();
    }
}
