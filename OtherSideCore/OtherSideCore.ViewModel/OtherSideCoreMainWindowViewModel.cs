using CommunityToolkit.Mvvm.ComponentModel;
using System;

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
         get => m_ApplicationLogoImageSource;
         set => SetProperty(ref m_ApplicationLogoImageSource, value);
      }

      public string ApplicationName
      {
         get => m_ApplicationName;
         set => SetProperty(ref m_ApplicationName, value);
      }

      public bool IsNavigationMenuDisplayed
      {
         get => m_IsNavigationMenuDisplayed;
         set => SetProperty(ref m_IsNavigationMenuDisplayed, value);
      }

      public ModelViewModel ModelViewModel
      {
         get => m_ModelViewModel;
         set => SetProperty(ref m_ModelViewModel, value);
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
