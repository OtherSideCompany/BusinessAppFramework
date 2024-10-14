using Microsoft.Extensions.Logging;
using OtherSideCore.Adapter.ViewDescriptions;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.Services;
using System.Reflection.Metadata.Ecma335;

namespace OtherSideCore.Adapter.Views
{
   public abstract class WorkspaceViewModel : ViewViewModelBase
   {
      #region Fields

      private WorkspaceDescription _workspaceDescription;

      protected IGlobalDataService _globalDataService;
      protected IDomainObjectViewModelFactory _viewModelFactory;
      protected IDomainObjectQueryServiceFactory _domainObjectQueryServiceFactory;
      protected IDomainObjectServiceFactory _domainObjectServiceFactory;      

      #endregion

      #region Properties

      public WorkspaceDescription WorkspaceDescription
      {
         get => _workspaceDescription;
         set => SetProperty(ref _workspaceDescription, value);
      }

      public virtual bool HasUnsavedChanges => false;

      #endregion

      #region Commands



      #endregion

      #region Constructor

      protected WorkspaceViewModel(ILoggerFactory loggerFactory, 
                                   IUserContext userContext, 
                                   IUserDialogService userDialogService,
                                   IGlobalDataService globalDataService,
                                   IDomainObjectViewModelFactory viewModelFactory,
                                   IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory,
                                   IDomainObjectServiceFactory domainObjectServiceFactory) : 
         base(loggerFactory, 
              userContext, 
              userDialogService)
      {
         _globalDataService = globalDataService;
         _viewModelFactory = viewModelFactory;
         _domainObjectQueryServiceFactory = domainObjectQueryServiceFactory;
         _domainObjectServiceFactory = domainObjectServiceFactory;
      }

      #endregion

      #region Public Methods      



      #endregion

      #region Private Methods



      #endregion

   }
}
