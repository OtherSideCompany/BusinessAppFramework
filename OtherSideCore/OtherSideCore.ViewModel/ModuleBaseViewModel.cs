using OtherSideCore.Model;
using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OtherSideCore.ViewModel
{
   public class ModuleBaseViewModel : ObservableObject, IDisposable
   {
      #region Fields

      private ModuleBase m_ModuleBase;
      private Type m_ViewModelType;
      private string m_ViewType;
      private string m_ViewAssembly;
      private UserControl m_ModuleView;
      private object m_IconResource;

      #endregion

      #region Properties

      public ModuleBase ModuleBase
      {
         get
         {
            return m_ModuleBase;
         }
         protected set
         {
            if (value != m_ModuleBase)
            {
               m_ModuleBase = value;
               OnPropertyChanged(nameof(ModuleBase));
            }
         }
      }

      public Type ViewModelType
      {
         get
         {
            return m_ViewModelType;
         }
         protected set
         {
            if (value != m_ViewModelType)
            {
               m_ViewModelType = value;
               OnPropertyChanged(nameof(ViewModelType));
            }
         }
      }

      public string ViewAssembly
      {
         get
         {
            return m_ViewAssembly;
         }
         protected set
         {
            if (value != m_ViewAssembly)
            {
               m_ViewAssembly = value;
               OnPropertyChanged(nameof(ViewAssembly));
            }
         }
      }

      public string ViewType
      {
         get
         {
            return m_ViewType;
         }
         protected set
         {
            if (value != m_ViewType)
            {
               m_ViewType = value;
               OnPropertyChanged(nameof(ViewType));
            }
         }
      }

      public UserControl ModuleView
      {
         get
         {
            return m_ModuleView;
         }
         protected set
         {
            if (value != m_ModuleView)
            {
               m_ModuleView = value;
               OnPropertyChanged(nameof(ModuleView));
            }
         }
      }

      public object IconResource
      {
         get
         {
            return m_IconResource;
         }
         protected set
         {
            if (value != m_IconResource)
            {
               m_IconResource = value;
               OnPropertyChanged(nameof(IconResource));
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ModuleBaseViewModel(ModuleBase moduleBase, Type viewModelType, string viewAssembly, string viewType, object iconResource)
      {
         ModuleBase = moduleBase;
         ViewModelType = viewModelType;
         ViewAssembly = viewAssembly;
         ViewType = viewType;
         IconResource = iconResource;
      }

      #endregion

      #region Methods

      public void Load()
      {
         if (ViewModelType == typeof(ModuleGroupModuleListViewModel))
         {
            var moduleGroupViewModel = Activator.CreateInstance(ViewModelType, this);
            ModuleView = (UserControl)Activator.CreateInstance(ViewAssembly, ViewType).Unwrap();
            ModuleView.DataContext = moduleGroupViewModel;
         }
         else
         {
            var moduleViewModel = Activator.CreateInstance(ViewModelType);
            ModuleView = (UserControl)Activator.CreateInstance(ViewAssembly, ViewType).Unwrap();
            ModuleView.DataContext = moduleViewModel;
         }
      }

      public void Load(List<string> filters)
      {
         var moduleViewModel = Activator.CreateInstance(ViewModelType, filters);
         ModuleView = (UserControl)Activator.CreateInstance(ViewAssembly, ViewType).Unwrap();
         ModuleView.DataContext = moduleViewModel;
      }

      public void Unload()
      {
         if (ModuleView != null)
         {
            (ModuleView.DataContext as IDisposable).Dispose();
            GC.Collect();
         }
      }

      public virtual void Dispose()
      {
         Unload();
         ModuleBase.Dispose();
      }


      #endregion
   }
}
