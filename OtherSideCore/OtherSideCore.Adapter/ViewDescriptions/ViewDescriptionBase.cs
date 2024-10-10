using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace OtherSideCore.Adapter.ViewDescriptions
{
   public abstract class ViewDescriptionBase : ObservableObject, IDisposable
   {
      #region Fields

      private bool _isLoaded;
      private string _name;
      private string _viewNavigationPath;
      private object _iconResource;

      protected Type _viewModelType;

      #endregion

      #region Properties

      public bool IsLoaded
      {
         get => _isLoaded;
         private set => SetProperty(ref _isLoaded, value);
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

      public object IconResource
      {
         get => _iconResource;
         protected set => SetProperty(ref _iconResource, value);
      }

      public Type ViewModelType
      {
         get => _viewModelType;
         protected set => SetProperty(ref _viewModelType, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ViewDescriptionBase(string name, Type viewModelType, object iconResource)
      {
         Name = name;
         _viewModelType = viewModelType;
         IconResource = iconResource;
      }

      #endregion

      #region Public Methods

      public void Load()
      {
         IsLoaded = true;
      }

      public void Unload()
      {
         IsLoaded = false;
      }

      public virtual void Dispose()
      {
         
      }

      #endregion
   }
}
