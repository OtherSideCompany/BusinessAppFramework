using OtherSideCore.Adapter.Services;
using OtherSideCore.Application;
using System;
using System.Collections.Generic;
using System.Windows;

namespace OtherSideCore.Wpf.Services
{
   public abstract class ViewLocatorService : IViewLocatorService
   {
      #region Fields

      private readonly Dictionary<StringKey, Type> _mappings = new();

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ViewLocatorService()
      {

      }

      #endregion

      #region Public Methods

      public void Register(StringKey viewKey, Type viewType)
      {
         _mappings[viewKey] = viewType;
      }

      public object ResolveView(StringKey viewKey, object viewModel)
      {
         var vmType = viewModel.GetType();

         if (_mappings.TryGetValue(viewKey, out var viewType))
         {
            var view = (FrameworkElement)Activator.CreateInstance(viewType)!;
            view.DataContext = viewModel;
            return view;
         }
         else
         {
            throw new Exception($"No view registered for ViewModel type {vmType.FullName}");
         }
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
