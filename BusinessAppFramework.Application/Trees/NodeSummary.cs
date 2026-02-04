using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Trees
{
   public class NodeSummary
   {
      #region Fields



      #endregion

      #region Properties

      public string ReferenceNumber { get; set; } = GlobalVariables.DefaultString;
      public string Title { get; set; } = GlobalVariables.DefaultString;
      public string SubTitle { get; set; } = GlobalVariables.DefaultString;

      #endregion

      #region Events



      #endregion

      #region Constructor

      public NodeSummary()
      {

      }

      public NodeSummary(string referenceNumber, string title, string subtitle)
      {
         ReferenceNumber = referenceNumber;
         Title = title;
         SubTitle = subtitle;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
