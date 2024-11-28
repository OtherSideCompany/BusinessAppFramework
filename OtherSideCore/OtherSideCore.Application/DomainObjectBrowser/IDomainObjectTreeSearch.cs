using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.DomainObjectBrowser
{
   public interface IDomainObjectTreeSearch : IDisposable
   {
      Task SearchAsync(DomainObject parent);
      void Dispose();
   }
}
