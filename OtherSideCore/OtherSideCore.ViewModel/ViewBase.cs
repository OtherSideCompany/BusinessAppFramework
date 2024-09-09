using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace OtherSideCore.ViewModel
{
   public abstract class ViewBase : ObservableObject, IDisposable
   {
      #region Fields
      
      private bool _isLoaded;
      private string _name;
      private string _viewNavigationPath;      
      private ViewModelBase _viewModel;
      private object _iconResource;

      protected Type _viewModelType;
      protected IServiceProvider _serviceProvider;
      protected ILoggerFactory _loggerFactory;
      protected ILogger _logger;

      #endregion

      #region Properties

      public bool IsLoaded
      {
         get => _isLoaded;
         set => SetProperty(ref _isLoaded, value);
      }

      public string Name
      {
         get => _name;
         set => SetProperty(ref _name, value);
      }

      public string ViewNavigationPath
      {
         get => _viewNavigationPath;
         set => SetProperty(ref _viewNavigationPath, value);
      }

      public ViewModelBase ViewModel
      {
         get => _viewModel;
         set => SetProperty(ref _viewModel, value);
      }

      public object IconResource
      {
         get => _iconResource;
         protected set => SetProperty(ref _iconResource, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ViewBase(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, string name, Type viewModelType, object iconResource)
      {
         _serviceProvider = serviceProvider;
         _loggerFactory = loggerFactory;
         _logger = loggerFactory.CreateLogger(GetType());

         Name = name;
         _viewModelType = viewModelType;
         IconResource = iconResource;
      }

      #endregion

      #region Public Methods

      public void Load(List<string> filters = null)
      {
         IsLoaded = true;
      }

      public void Unload()
      {
         ViewModel?.Dispose();
         ViewModel = null;

         IsLoaded = false;
      }

      public void InstanciateViewModel()
      {
         if (_viewModelType == null)
         {
            _logger.LogInformation("Displaying view {ViewName}", Name);
         }
         else
         {
            ViewModel = (ViewModelBase)_serviceProvider.GetService(_viewModelType);
            _logger.LogInformation("Displaying view {ViewName} in {ViewModelType}", Name, _viewModelType.Name);
         }
         
      }

      public virtual void Dispose()
      {
         Unload();
      }

      #endregion
   }
}
