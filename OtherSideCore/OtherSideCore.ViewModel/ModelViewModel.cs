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

      private ObservableCollection<ModuleBaseViewModel> m_QuickNavigationModuleBases;

      #endregion

      #region Properties

      public ObservableCollection<ModuleBaseViewModel> QuickNavigationModuleBases
      {
         get
         {
            return m_QuickNavigationModuleBases;
         }
         set
         {
            if (value != m_QuickNavigationModuleBases)
            {
               m_QuickNavigationModuleBases = value;
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
         QuickNavigationModuleBases = new ObservableCollection<ModuleBaseViewModel>();
      }

      #endregion

      #region Methods



      #endregion
   }
}
