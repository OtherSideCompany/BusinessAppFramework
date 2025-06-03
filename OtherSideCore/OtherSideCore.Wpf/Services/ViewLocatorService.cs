using System;
using System.Collections.Generic;
using System.Windows;

namespace OtherSideCore.Wpf.Services
{
   public class ViewLocatorService : IViewLocatorService
   {
      #region Fields

      private readonly Dictionary<Type, Type> _mappings = new();

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

      public void Register(Type viewModelType, Type viewType)
      {
         _mappings[viewModelType] = viewType;
      }

      public object ResolveView(object viewModel)
      {
         var vmType = viewModel.GetType();

         if (_mappings.TryGetValue(vmType, out var viewType))
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
