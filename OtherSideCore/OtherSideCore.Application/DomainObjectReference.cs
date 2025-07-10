using OtherSideCore.Domain;

namespace OtherSideCore.Application
{
   public class DomainObjectReference
   {
      #region Fields



      #endregion

      #region Properties

      public int DomainObjectId { get; set; }
      public string Name { get; set; }
      public Type ReferenceType { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectReference(int domainObjectId, string name, Type referenceType)
      {
         DomainObjectId = domainObjectId;
         Name = name;
         ReferenceType = referenceType;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
