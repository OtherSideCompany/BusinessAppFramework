using CommunityToolkit.Mvvm.ComponentModel;
using System;

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
         get => m_ModuleGroupViewModel;
         set => SetProperty(ref m_ModuleGroupViewModel, value);
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
