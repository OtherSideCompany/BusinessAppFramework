namespace OtherSideCore.Application.Factories
{
   public interface IDomainObjectReferenceFactory
   {
      void RegisterDomainObjectReference<TEntity>(Func<TEntity, DomainObjectReference> factory) where TEntity : IEntity;
      DomainObjectReference CreateDomainObjectReference(IEntity entityBase);
   }
}
