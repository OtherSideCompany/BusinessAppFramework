using OtherSideCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public interface IModelObjectViewModeFactory
   {
      ModelObjectViewModel CreateViewModel(ModelObject modelObject);
   }
}
