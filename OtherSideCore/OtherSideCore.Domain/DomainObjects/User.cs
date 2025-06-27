using System.ComponentModel.DataAnnotations;

namespace OtherSideCore.Domain.DomainObjects
{
   public class User : DomainObject
   {
      #region Fields


      #endregion

      #region Properties

      [MaxLength(50)]
      public string UserName { get; set; } = GlobalVariables.DefaultString;

      [MaxLength(64), MinLength(64)]
      public string PasswordHash { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public User() : base()
      {
         
      }

      #endregion

      #region Public Methods

      public override string ToString()
      {
         return UserName;
      }

      #endregion
   }
}
