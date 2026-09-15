using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Factories
{
   /// <summary>
   /// Instantiates domain objects, honouring the type substitutions registered by custom projects
   /// (see IDomainObjectTypeMap.RegisterSubstitution). Application code must use it instead of
   /// <c>new</c> so that an extended type is created wherever its base type is requested.
   /// </summary>
   public interface IDomainObjectFactory
   {
      T Create<T>() where T : DomainObject;
      DomainObject Create(Type domainObjectType);
   }
}
