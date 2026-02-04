namespace BusinessAppFramework.Domain.DomainObjects
{
   public class DomainObjectReferenceListItem
   {
      #region Fields



      #endregion

      #region Properties

      public int DomainObjectId { get; set; }
      public string DisplayValue { get; set; }

      #endregion

      #region Events



      #endregion

      #region Constructor

      public DomainObjectReferenceListItem()
      {

      }

      public DomainObjectReferenceListItem(int domainObjectId, string displayValue)
      {
         DomainObjectId = domainObjectId;
         DisplayValue = displayValue;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
