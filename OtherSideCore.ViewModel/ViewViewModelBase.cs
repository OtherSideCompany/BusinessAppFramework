using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Repositories;
using OtherSideCore.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public abstract class ViewViewModelBase : ObservableObject, IDisposable
   {
      #region Fields

      protected IAuthenticationService _authenticationService;
      protected IRepositoryFactory _repositoryFactory;
      protected ILoggerFactory _loggerFactory;
      protected IGlobalDataService _globalDataService;
      protected IModelObjectViewModelFactory _modelObjectViewModeFactory;
      protected IModelObjectFactory _modelObjectFactory;

      #endregion

      #region Properties


      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ViewViewModelBase(IAuthenticationService authenticationService, IRepositoryFactory repositoryFactory, IModelObjectViewModelFactory modelObjectViewModeFactory, ILoggerFactory loggerFactory, IGlobalDataService globalDataService, IModelObjectFactory modelObjectFactory)
      {
         _authenticationService = authenticationService;
         _repositoryFactory = repositoryFactory;
         _loggerFactory = loggerFactory;
         _globalDataService = globalDataService;
         _modelObjectViewModeFactory = modelObjectViewModeFactory;
         _modelObjectFactory = modelObjectFactory;
      }

      #endregion

      #region Public Methods

      public abstract Task InitializeAsync(CancellationToken cancellationToken);

      public abstract bool HasUnsavedChanges();

      public abstract void Dispose();

      #endregion

      #region Private Methods

      protected virtual void NotifyCommandsCanExecuteChanged()
      {

      }

      #endregion
   }
}
