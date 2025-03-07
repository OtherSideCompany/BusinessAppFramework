using System.Collections.Generic;

namespace OtherSideCore.Domain.DomainObjects
{
   public class CommentThread : DomainObject
   {
      #region Fields


      #endregion

      #region Properties

      public List<Comment> Comments { get; set; } = new List<Comment>();

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public CommentThread()
      {
         
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
