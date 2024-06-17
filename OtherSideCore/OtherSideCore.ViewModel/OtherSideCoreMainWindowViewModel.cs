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
   public abstract class OtherSideCoreMainWindowViewModel : ObservableObject, IDisposable
   {
      #region Fields

      private string m_ApplicationLogoImageSource;
      private string m_ApplicationName;
      private bool m_IsNavigationMenuDisplayed;
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
               OnPropertyChanged(nameof(ApplicationLogoImageSource));
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
               OnPropertyChanged(nameof(ApplicationName));
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
               OnPropertyChanged(nameof(IsNavigationMenuDisplayed));
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
               OnPropertyChanged(nameof(ModelViewModel));
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
      }

      #endregion

      #region Methods

      public void Dispose()
      {
         ModelViewModel.Dispose();
         ModelViewModel = null;
      }

      #endregion
   }
}
