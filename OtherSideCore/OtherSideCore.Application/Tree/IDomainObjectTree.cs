using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Tree
{
    public interface IDomainObjectTree : IDisposable
    {
        Task FillDomainObjectAsync(DomainObject parent);
        void Dispose();
    }
}
