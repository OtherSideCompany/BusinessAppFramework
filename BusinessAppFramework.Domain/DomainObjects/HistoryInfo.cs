using System;

namespace BusinessAppFramework.Domain.DomainObjects
{
   public class HistoryInfo
   {
      #region Fields



      #endregion

      #region Properties

      public DateTime CreationDate { get; set; }
      public int? CreatedById { get; set; }
      public string CreatedByName { get; set; }
      public DateTime LastModifiedDateTime { get; set; }
      public int? LastModifiedById { get; set; }
      public string LastModifiedByName { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public HistoryInfo()
      {

      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
