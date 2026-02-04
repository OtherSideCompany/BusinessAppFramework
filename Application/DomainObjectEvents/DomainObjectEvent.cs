namespace Application.DomainObjectEvents
{
   public abstract class DomainObjectEvent
   {
      #region Fields



      #endregion

      #region Properties

      public Type DomainObjectType { get; private set; }
      public int DomainObjectId { get; private set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectEvent(Type domainObjectType, int domainObjectId)
      {
         DomainObjectType = domainObjectType;
         DomainObjectId = domainObjectId;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
