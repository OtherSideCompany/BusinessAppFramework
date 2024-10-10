using OtherSideCore.Domain.DomainObjects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OtherSideCore.Domain.DomainObjects
{
   public class User : DomainObject
   {
      #region Fields


      #endregion

      #region Properties

      public bool IsActive { get; set; }

      [MaxLength(50)]
      public string FirstName { get; set; } = GlobalVariables.DefaultString;

      [MaxLength(50)]
      public string LastName { get; set; } = GlobalVariables.DefaultString;

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
         IsActive = true;
      }

      #endregion

      #region Public Methods

      

      #endregion
   }
}
