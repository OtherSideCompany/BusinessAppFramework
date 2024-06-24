using OtherSideCore.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Threading.Tasks;

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
         get => m_ModuleBase;
         set => SetProperty(ref m_ModuleBase, value);
      }

      public Type ViewModelType
      {
         get => m_ViewModelType;
         protected set => SetProperty(ref m_ViewModelType, value);
      }

      public string ViewAssembly
      {
         get => m_ViewAssembly;
         protected set => SetProperty(ref m_ViewAssembly, value);
      }

      public string ViewType
      {
         get => m_ViewType;
         protected set => SetProperty(ref m_ViewType, value);
      }

      public UserControl ModuleView
      {
         get => m_ModuleView;
         protected set => SetProperty(ref m_ModuleView, value);
      }

      public object IconResource
      {
         get => m_IconResource;
         protected set => SetProperty(ref m_IconResource, value);
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
