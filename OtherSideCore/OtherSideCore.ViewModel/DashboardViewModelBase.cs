using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Repositories;
using OtherSideCore.Domain.Services;
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

      public DashboardViewModelBase(IAuthenticationService authenticationService, IRepositoryFactory repositoryFactory, IModelObjectViewModelFactory modelObjectViewModeFactory, ILoggerFactory loggerFactory, IGlobalDataService globalDataService) : base(authenticationService, repositoryFactory, modelObjectViewModeFactory, loggerFactory, globalDataService)
      {
         
      }

      #endregion

      #region Public Methods

      public override bool HasUnsavedChanges()
      {
         return false;
      }

      public virtual void Dispose()
      {
         
      }      

      #endregion

      #region Private Methods



      #endregion
   }
}
