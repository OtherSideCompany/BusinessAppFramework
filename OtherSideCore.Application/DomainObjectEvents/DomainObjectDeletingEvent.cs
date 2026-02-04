using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.DomainObjectEvents
{
   public class DomainObjectDeletingEvent : DomainObjectEvent
   {
      #region Fields



      #endregion

      #region Properties

      

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectDeletingEvent(Type domainObjectType, int domainObjectId) : base(domainObjectType, domainObjectId)
      {
         
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
