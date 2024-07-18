using OtherSideCore.Model.ModelObjects;
using OtherSideCore.ViewModel.ModelObjectViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
    public interface IModelObjectViewModelFactory
   {
      ModelObjectViewModel CreateViewModel(ModelObject modelObject);
   }
}
