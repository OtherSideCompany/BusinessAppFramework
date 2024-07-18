using OtherSideCore.Model.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel.ModelObjectViewModels
{
   public class UserViewModel : ModelObjectViewModel
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public UserViewModel(User user) : base(user)
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
