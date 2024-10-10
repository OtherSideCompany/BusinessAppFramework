using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;

namespace OtherSideCore.Adapter.Views
{
   public abstract class ViewViewModelBase : ObservableObject, IDisposable
   {
      #region Fields

      protected IUserContext _userContext;
      //protected IRepositoryFactory _repositoryFactory;
      protected ILoggerFactory _loggerFactory;
      //protected IGlobalDataService _globalDataService;
      //protected IModelObjectViewModelFactory _modelObjectViewModeFactory;
      //protected IModelObjectFactory _modelObjectFactory;
      protected IUserDialogService _userDialogService;
      protected IDomainObjectViewModelFactory _viewModelFactory;

      #endregion

      #region Properties


      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ViewViewModelBase(ILoggerFactory loggerFactory, IUserContext userContext, IUserDialogService userDialogService, IDomainObjectViewModelFactory viewModelFactory)
      {
         _userContext = userContext;
         //_repositoryFactory = repositoryFactory;
         _loggerFactory = loggerFactory;
         //_globalDataService = globalDataService;
         //_modelObjectViewModeFactory = modelObjectViewModeFactory;
         //_modelObjectFactory = modelObjectFactory;
         _userDialogService = userDialogService;
         _viewModelFactory = viewModelFactory;
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
