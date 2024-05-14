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
   public class ModuleBaseViewModel : ObservableObject
   {
      #region Fields

      private ModuleBase m_ModuleBase;
      private Type m_ViewModelType;
      private Type m_ViewType;
      private UserControl m_ModuleView;
      private string m_IconFilePath;

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
               OnPropertyChanged("ModuleBase");
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
               OnPropertyChanged("ViewModelType");
            }
         }
      }

      public Type ViewType
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
               OnPropertyChanged("ViewType");
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
               OnPropertyChanged("ModuleView");
            }
         }
      }

      public string IconFilePath
      {
         get
         {
            return m_IconFilePath;
         }
         protected set
         {
            if (value != m_IconFilePath)
            {
               m_IconFilePath = value;
               OnPropertyChanged("IconFilePath");
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ModuleBaseViewModel(ModuleBase moduleBase, Type viewModelType, Type viewType, string iconFilePath)
      {
         ModuleBase = moduleBase;
         ViewModelType = viewModelType;
         ViewType = viewType;
         IconFilePath = iconFilePath;
      }

      #endregion

      #region Methods

      public void Load()
      {
         if (ViewModelType == typeof(ModuleGroupViewModel))
         {
            var moduleGroupViewModel = (ModuleGroupViewModel)Activator.CreateInstance(ViewModelType, this);
            ModuleView = (UserControl)Activator.CreateInstance(m_ViewType);
            ModuleView.DataContext = moduleGroupViewModel;
         }
         else
         {
            var moduleViewModel = (ModuleBaseViewModel)Activator.CreateInstance(ViewModelType);
            ModuleView = (UserControl)Activator.CreateInstance(m_ViewType);
            ModuleView.DataContext = moduleViewModel;
         }

         ModuleBase.Load();
      }

      public void Load(List<string> filters)
      {
         var moduleViewModel = (ModuleBaseViewModel)Activator.CreateInstance(ViewModelType, filters);
         ModuleView = (UserControl)Activator.CreateInstance(m_ViewType);
         ModuleView.DataContext = moduleViewModel;

         ModuleBase.Load();
      }

      public void Unload()
      {
         if (ModuleView != null)
         {
            (ModuleView.DataContext as IDisposable).Dispose();

            ModuleBase.Unload();

            GC.Collect();
         }
      }


      #endregion
   }
}
