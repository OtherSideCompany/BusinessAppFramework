using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Factories
{
   public class DomainObjectFactory : IDomainObjectFactory
   {
      #region Fields

      private readonly IDomainObjectTypeMap _domainObjectTypeMap;

      #endregion

      #region Constructor

      public DomainObjectFactory(IDomainObjectTypeMap domainObjectTypeMap)
      {
         _domainObjectTypeMap = domainObjectTypeMap;
      }

      #endregion

      #region Public Methods

      public T Create<T>() where T : DomainObject
      {
         return (T)Create(typeof(T));
      }

      public DomainObject Create(Type domainObjectType)
      {
         var resolvedType = _domainObjectTypeMap.ResolveDomainType(domainObjectType);

         return Activator.CreateInstance(resolvedType) as DomainObject
            ?? throw new InvalidOperationException($"Cannot instantiate domain object type {resolvedType.Name}");
      }

      #endregion
   }
}
