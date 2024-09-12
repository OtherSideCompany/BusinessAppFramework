using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace OtherSideCore.ViewModel
{
   public class ViewDescription : ViewDescriptionBase
   {
      #region Fields

      private DashboardDescription _parentViewGroup;
      private ViewViewModelBase _viewViewModelBase;

      #endregion

      #region Properties

      public DashboardDescription ParentViewGroup
      {
         get => _parentViewGroup;
         set => SetProperty(ref _parentViewGroup, value);
      }

      public ViewViewModelBase ViewViewModelBase
      {
         get => _viewViewModelBase;
         set => SetProperty(ref _viewViewModelBase, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ViewDescription(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, string name, Type viewModelType, object iconResource, DashboardDescription parent = null) : base(serviceProvider, loggerFactory, name, viewModelType, iconResource)
      {
         ParentViewGroup = parent;

         if (ParentViewGroup != null)
         {
            ViewNavigationPath = parent.Name + " > " + Name;
         }
         else
         {
            ViewNavigationPath = Name;
         }
      }

      #endregion

      #region Public Methods

      public override void InstanciateViewModel()
      {
         ViewViewModelBase = (ViewViewModelBase)_serviceProvider.GetService(_viewModelType);
         _logger.LogInformation("Displaying view {ViewName} with view model {ViewModelType}", Name, _viewModelType.Name);
      }

      public override void Unload()
      {
         base.Unload();

         ViewViewModelBase?.Dispose();
         ViewViewModelBase = null;
      }

      #endregion
   }
}
