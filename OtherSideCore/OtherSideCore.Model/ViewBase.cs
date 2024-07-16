using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace OtherSideCore.Model
{
   public abstract class ViewBase : ObservableObject, IDisposable
   {
      #region Fields

      private bool _isLoaded;
      private string _name;
      private string _viewNavigationPath;
      private Type _viewModelType;
      private object _iconResource;

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

      public Type ViewModelType
      {
         get => _viewModelType;
         protected set => SetProperty(ref _viewModelType, value);
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

      public ViewBase(string name, Type viewModelType, object iconResource)
      {
         Name = name;
         ViewModelType = viewModelType;
         IconResource = iconResource;
      }

      #endregion

      #region Methods

      public void Load(List<string> filters = null)
      {
         IsLoaded = true;
      }

      public void Unload()
      {
         IsLoaded = false;
      }

      public virtual void Dispose()
      {
         Unload();
      }

      #endregion
   }
}
