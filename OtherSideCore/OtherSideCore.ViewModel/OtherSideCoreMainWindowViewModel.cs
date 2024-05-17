using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OtherSideCore.ViewModel
{
   public abstract class OtherSideCoreMainWindowViewModel : ObservableObject
   {
      #region Fields

      private string m_ApplicationLogoImageSource;
      private string m_ApplicationName;
      private bool m_IsNavigationMenuDisplayed;
      private ModuleBaseViewModel m_CurrentLoadedModuleBaseViewModel;
      private SolidColorBrush m_LogoBackgroundColor;
      private SolidColorBrush m_HeaderBarBackgroundColor;
      private ModelViewModel m_ModelViewModel;

      #endregion

      #region Properties

      public string ApplicationLogoImageSource
      {
         get
         {
            return m_ApplicationLogoImageSource;
         }
         set
         {
            if (value != m_ApplicationLogoImageSource)
            {
               m_ApplicationLogoImageSource = value;
               OnPropertyChanged("ApplicationLogoImageSource");
            }
         }
      }

      public string ApplicationName
      {
         get
         {
            return m_ApplicationName;
         }
         set
         {
            if (value != m_ApplicationName)
            {
               m_ApplicationName = value;
               OnPropertyChanged("ApplicationName");
            }
         }
      }

      public bool IsNavigationMenuDisplayed
      {
         get
         {
            return m_IsNavigationMenuDisplayed;
         }
         set
         {
            if (value != m_IsNavigationMenuDisplayed)
            {
               m_IsNavigationMenuDisplayed = value;
               OnPropertyChanged("IsNavigationMenuDisplayed");
            }
         }
      }

      public ModuleBaseViewModel CurrentLoadedModuleBaseViewModel
      {
         get
         {
            return m_CurrentLoadedModuleBaseViewModel;
         }
         set
         {
            if (value != m_CurrentLoadedModuleBaseViewModel)
            {
               m_CurrentLoadedModuleBaseViewModel = value;
               OnPropertyChanged("CurrentLoadedModuleBaseViewModel");
            }
         }
      }

      public SolidColorBrush LogoBackgroundColor
      {
         get
         {
            return m_LogoBackgroundColor;
         }
         set
         {
            if (value != m_LogoBackgroundColor)
            {
               m_LogoBackgroundColor = value;
               OnPropertyChanged("LogoBackgroundColor");
            }
         }
      }

      public SolidColorBrush HeaderBarBackgroundColor
      {
         get
         {
            return m_HeaderBarBackgroundColor;
         }
         set
         {
            if (value != m_HeaderBarBackgroundColor)
            {
               m_HeaderBarBackgroundColor = value;
               OnPropertyChanged("HeaderBarBackgroundColor");
            }
         }
      }

      public ModelViewModel ModelViewModel
      {
         get
         {
            return m_ModelViewModel;
         }
         set
         {
            if (value != m_ModelViewModel)
            {
               m_ModelViewModel = value;
               OnPropertyChanged("ModelViewModel");
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public OtherSideCoreMainWindowViewModel()
      {
         ApplicationName = "Unnamed App";

         LogoBackgroundColor = new SolidColorBrush(Colors.Black);
         HeaderBarBackgroundColor = new SolidColorBrush(Color.FromRgb(23,25,28));
      }

      #endregion

      #region Methods



      #endregion
   }
}
