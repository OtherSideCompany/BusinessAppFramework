using OtherSideCore.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public class ModuleGroupViewModel : ModuleBaseViewModel
   {
      #region Fields

      private bool m_IsExpanded;

      private ObservableCollection<ModuleViewModel> m_ModuleViewModels;

      #endregion

      #region Properties

      public ModuleGroup ModuleGroup
      {
         get
         {
            return ModuleBase as ModuleGroup;
         }
      }
         

      public bool IsExpanded
      {
         get
         {
            return m_IsExpanded;
         }
         set
         {
            if (value != m_IsExpanded)
            {
               m_IsExpanded = value;
               OnPropertyChanged("IsExpanded");
            }
         }
      }

      public ObservableCollection<ModuleViewModel> ModuleViewModels
      {
         get
         {
            return m_ModuleViewModels;
         }
         set
         {
            if (value != m_ModuleViewModels)
            {
               m_ModuleViewModels = value;
               OnPropertyChanged("ModuleViewModels");
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ModuleGroupViewModel(ModuleGroup moduleGroup, Type viewModelType, string viewAssembly, string viewType, object iconResource) : base(moduleGroup, viewModelType, viewAssembly, viewType, iconResource)
      {
         ModuleViewModels = new ObservableCollection<ModuleViewModel>();
      }

      #endregion

      #region Methods



      #endregion
   }
}
