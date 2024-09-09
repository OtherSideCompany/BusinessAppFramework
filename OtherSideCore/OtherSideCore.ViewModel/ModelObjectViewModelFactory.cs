using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.ViewModel.ModelObjectViewModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public class ModelObjectViewModelFactory : IModelObjectViewModelFactory
   {
      public virtual ModelObjectViewModel CreateViewModel(ModelObject modelObject)
      {
         if (modelObject is User)
         {
            return CreateUserViewModel(modelObject as User);
         }
         else
         {
            return new ModelObjectViewModel(modelObject);
         }
      }

      protected virtual UserViewModel CreateUserViewModel(User user)
      {
         return new UserViewModel(user);
      }
   }
}
