namespace Domain.DomainObjects
{
   public class DomainObjectReference
   {
      #region Fields



      #endregion

      #region Properties

      public string RelationKey { get; set; }
      public int? DomainObjectId { get; set; }
      public string DisplayValue { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectReference()
      {
         RelationKey = string.Empty;
      }

      public DomainObjectReference(string relationKey, int? domainObjectId)
      {
         DomainObjectId = domainObjectId;
         RelationKey = relationKey;
      }

      #endregion

      #region Public Methods


      #endregion

      #region Private Methods



      #endregion
   }
}
