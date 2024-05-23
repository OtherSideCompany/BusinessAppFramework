using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public class ModelViewModel : ObservableObject
   {
      #region Fields

      private ObservableCollection<ModuleBaseViewModel> m_QuickNavigationModuleViewModels;

      #endregion

      #region Properties

      public ObservableCollection<ModuleBaseViewModel> QuickNavigationModuleViewModels
      {
         get
         {
            return m_QuickNavigationModuleViewModels;
         }
         set
         {
            if (value != m_QuickNavigationModuleViewModels)
            {
               m_QuickNavigationModuleViewModels = value;
               OnPropertyChanged("QuickNavigationModuleBases");
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ModelViewModel()
      {
         QuickNavigationModuleViewModels = new ObservableCollection<ModuleBaseViewModel>();
      }

      #endregion

      #region Methods



      #endregion
   }
}
