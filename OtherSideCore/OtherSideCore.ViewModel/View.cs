using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace OtherSideCore.ViewModel
{
   public class View : ViewBase
   {
      #region Fields

      private ViewGroup _parentViewGroup;

      #endregion

      #region Properties

      public ViewGroup ParentViewGroup
      {
         get => _parentViewGroup;
         set => SetProperty(ref _parentViewGroup, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public View(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, string name, Type viewModelType, object iconResource, ViewGroup parent = null) : base(serviceProvider, loggerFactory, name, viewModelType, iconResource)
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

      

      #endregion
   }
}
