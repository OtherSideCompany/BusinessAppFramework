using OtherSideCore.Application;

namespace OtherSideCore.Adapter.Factories
{
   public interface IDomainObjectReferenceFactory
   {
      DomainObjectReference CreateDomainObjectReference(IEntity entityBase);
   }
}
