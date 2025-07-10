using OtherSideCore.Adapter;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Application;
using OtherSideCore.Application.Factories;
using System;

namespace OtherSideCore.Infrastructure.Factories
{
   public class DomainObjectReferenceFactory : TypeBasedFactory, IDomainObjectReferenceFactory
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Constructor

      public DomainObjectReferenceFactory()
      {

      }

      #endregion

      #region Public Methods

      public void RegisterDomainObjectReference<TEntity>(Func<TEntity, DomainObjectReference> factory) where TEntity : IEntity
      {
         Register<TEntity>(args =>
         {
            var entity = (TEntity)args[0]!;
            return factory(entity);
         });
      }

      public DomainObjectReference CreateDomainObjectReference(IEntity entityBase)
      {
         return (DomainObjectReference)CreateFromType(entityBase.GetType(), entityBase);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
