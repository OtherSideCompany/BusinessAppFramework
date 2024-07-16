using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using OtherSideCore.Model.ModelObjects;
using OtherSideCore.Model.Services;
using System;

namespace OtherSideCore.ViewModel
{
   public abstract class MainWindowViewModel<T, U> : ObservableObject, IDisposable where T : User where U : DbContext
   {
      #region Fields

      protected IAuthenticationService<T> _authenticationService;
      protected IDbContextFactory<U> _contextFactory;

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

      public MainWindowViewModel(IAuthenticationService<T> authenticationService, IDbContextFactory<U> contextFactory)
      {
         _authenticationService = authenticationService;
         _contextFactory = contextFactory;

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
