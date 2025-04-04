using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.DomainObjectEvents
{
   public abstract class DomainObjectEvent
   {
      #region Fields



      #endregion

      #region Properties

      public DomainObject DomainObject { get; private set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectEvent(DomainObject domainObject)
      {
         DomainObject = domainObject;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
