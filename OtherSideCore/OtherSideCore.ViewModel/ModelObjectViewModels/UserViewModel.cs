using OtherSideCore.Model.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel.ModelObjectViewModels
{
    public abstract class UserViewModel : ModelObjectViewModel
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public UserViewModel(User user, User authenticatedUser) : base(user, authenticatedUser) 
      {

      }

      #endregion

      #region Methods

      protected override void DisplayInExternalWindow()
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}
