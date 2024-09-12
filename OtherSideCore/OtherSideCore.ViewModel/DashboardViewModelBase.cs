using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Domain.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public abstract class DashboardViewModelBase : ViewViewModelBase
   {
      #region Fields

      private DashboardDescription _dashboardDescription;

      #endregion

      #region Properties

      public DashboardDescription DashboardDescription
      {
         get => _dashboardDescription;
         set => SetProperty(ref _dashboardDescription, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DashboardViewModelBase()
      {
         
      }

      #endregion

      #region Public Methods

      public virtual void Dispose()
      {
         
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
