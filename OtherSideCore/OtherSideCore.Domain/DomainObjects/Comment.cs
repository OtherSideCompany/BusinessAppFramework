using System.ComponentModel.DataAnnotations;

namespace OtherSideCore.Domain.DomainObjects
{
   public class Comment : DomainObject
   {
      #region Fields



      #endregion

      #region Properties

      [MaxLength(2000)]
      public string Message { get; set; } = GlobalVariables.DefaultString;

      public User Author { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Comment()
      {
         
      }

      #endregion

      #region Public Methods

      public bool CanBeEditedBy(int authenticatedUserId)
      {
         return authenticatedUserId == Author.Id;
      }

      public bool CanBeDeletedBy(int authenticatedUserId)
      {
         return authenticatedUserId == Author.Id;
      }

      #endregion

      #region Private Methods

      

      #endregion
   }
}
