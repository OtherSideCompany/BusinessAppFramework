using OtherSideCore.Domain;

namespace OtherSideCore.Application
{
   public class DomainObjectReference
   {
      #region Fields



      #endregion

      #region Properties

      public int DomainObjectId { get; set; }
      public string ReferenceNumber { get; set; }
      public string Name { get; set; }
      public StringKey ReferenceKey { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectReference(int domainObjectId, string referenceNumber, string name, StringKey referenceKey)
      {
         DomainObjectId = domainObjectId;
         ReferenceNumber = referenceNumber;
         Name = name;
         ReferenceKey = referenceKey;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
