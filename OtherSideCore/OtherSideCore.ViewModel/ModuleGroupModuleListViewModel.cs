using OtherSideCore.Model;
using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public class ModuleGroupModuleListViewModel : ObservableObject, IDisposable
   {
      #region Fields

      private ModuleGroupViewModel m_ModuleGroupViewModel;

      #endregion

      #region Properties

      public ModuleGroupViewModel ModuleGroupViewModel
      {
         get
         {
            return m_ModuleGroupViewModel;
         }
         set
         {
            if (value != m_ModuleGroupViewModel)
            {
               m_ModuleGroupViewModel = value;
               OnPropertyChanged(nameof(ModuleGroupViewModel));
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ModuleGroupModuleListViewModel(ModuleGroupViewModel moduleGroupViewModel)
      {
         ModuleGroupViewModel = moduleGroupViewModel;
      }

      #endregion

      #region Methods

      public void Dispose()
      {

      }

      #endregion
   }
}
