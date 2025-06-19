using OtherSideCore.Adapter;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Application;
using OtherSideCore.Application.Factories;

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

      public DomainObjectReference CreateDomainObjectReference(IEntity entityBase)
      {
         return (DomainObjectReference)CreateFromType(entityBase.GetType());
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
