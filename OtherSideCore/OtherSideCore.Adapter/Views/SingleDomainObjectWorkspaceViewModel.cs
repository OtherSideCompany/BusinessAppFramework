using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;
using System.ComponentModel;

namespace OtherSideCore.Adapter.Views
{
   public abstract class SingleDomainObjectWorkspaceViewModel<T> : WorkspaceViewModel where T : DomainObject, new()
   {
      #region Fields

      protected DomainObjectsSearchViewModel<T> _domainObjectsSearchViewModel;
      protected DomainObjectEditorViewModel<T> _domainObjectsEditorViewModel;
      protected DomainObjectManagerViewModel<T> _domainObjectsManagerViewModel;
      protected IDomainObjectService<T> _domainObjectService;
      protected IDomainObjectQueryService<T> _domainObjectQueryService;
      protected IGlobalDataService _globalDataService;

      #endregion

      #region Properties

      public DomainObjectsSearchViewModel<T> DomainObjectsSearchViewModel
      {
         get => _domainObjectsSearchViewModel;
         set => SetProperty(ref _domainObjectsSearchViewModel, value);
      }

      public DomainObjectEditorViewModel<T> DomainObjectEditorViewModel
      {
         get => _domainObjectsEditorViewModel;
         set => SetProperty(ref _domainObjectsEditorViewModel, value);
      }

      public DomainObjectManagerViewModel<T> DomainObjectsManagerViewModel
      {
         get => _domainObjectsManagerViewModel;
         set => SetProperty(ref _domainObjectsManagerViewModel, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public SingleDomainObjectWorkspaceViewModel(ILoggerFactory loggerFactory, 
                                                  IUserContext userContext, 
                                                  IDomainObjectQueryService<T> domainObjectQueryService, 
                                                  IUserDialogService userDialogService, 
                                                  IDomainObjectService<T> domainObjectService, 
                                                  IDomainObjectViewModelFactory viewModelFactory,
                                                  IGlobalDataService globalDataService) : 
         base(loggerFactory, 
              userContext, 
              userDialogService, 
              viewModelFactory)
      {
         _domainObjectService = domainObjectService;
         _domainObjectQueryService = domainObjectQueryService;
         _globalDataService = globalDataService;

         DomainObjectsSearchViewModel = new DomainObjectsSearchViewModel<T>(userContext, _domainObjectQueryService, _domainObjectService, viewModelFactory) { SelectedSearchResultChangedAsync = SelectedSearchResultChangedAsync };
         DomainObjectsManagerViewModel = new DomainObjectManagerViewModel<T>(userContext, domainObjectService, userDialogService, viewModelFactory);

         DomainObjectsManagerViewModel.DomainObjectCreated += OnDomainObjectCreated;
         DomainObjectsManagerViewModel.DomainObjectDeleted += OnDomainObjectDeleted;
      }

      #endregion

      #region Public Methods

      public override async Task InitializeAsync(CancellationToken cancellationToken = default)
      {
         await DomainObjectsSearchViewModel.SearchAsync(cancellationToken);
      }

      public override bool HasUnsavedChanges()
      {
         return DomainObjectEditorViewModel != null && DomainObjectEditorViewModel.HasUnsavedChanges;
      }

      public override void Dispose()
      {
         DomainObjectsManagerViewModel.DomainObjectCreated -= OnDomainObjectCreated;
         DomainObjectsManagerViewModel.DomainObjectDeleted -= OnDomainObjectDeleted;

         DomainObjectsSearchViewModel.Dispose();
         DomainObjectsManagerViewModel.Dispose();
         UnloadEditorViewModel();
      }

      public virtual DomainObjectEditorViewModel<T> CreateDomainObjectEditorViewModel()
      {
         return new DomainObjectEditorViewModel<T>(_userContext, _domainObjectService, DomainObjectsSearchViewModel.SelectedSearchResultViewModel, _userDialogService);
      }

      #endregion

      #region Private Methods

      private void OnDomainObjectDeleted(object? sender, DomainObjectViewModel e)
      {
         DomainObjectsSearchViewModel.RemoveSearchResultViewModel(e);         
      }

      private async void OnDomainObjectCreated(object? sender, DomainObjectViewModel e)
      {
         DomainObjectsSearchViewModel.AddSearchResultViewModel(e);
         await DomainObjectsSearchViewModel.SelectSearchResultAsync(e, CancellationToken.None);
      }

      private void UnloadEditorViewModel()
      {
         if (DomainObjectEditorViewModel != null)
         {
            DomainObjectEditorViewModel.PropertyChanged -= UserEditorViewModel_PropertyChanged;
            DomainObjectEditorViewModel.Dispose();
            DomainObjectEditorViewModel = null;
         }
      }

      protected virtual async Task SelectedSearchResultChangedAsync(CancellationToken cancellation)
      {
         UnloadEditorViewModel();

         var selectedDomainObject = DomainObjectsSearchViewModel.SelectedSearchResultViewModel.DomainObject;

         await _domainObjectService.LoadTrackingInfosAsync((T)selectedDomainObject);

         if (selectedDomainObject != null)
         {
            DomainObjectEditorViewModel = CreateDomainObjectEditorViewModel();
            DomainObjectEditorViewModel.PropertyChanged += UserEditorViewModel_PropertyChanged;

            DomainObjectsManagerViewModel.SelectedDomainObjectViewModel = DomainObjectsSearchViewModel.SelectedSearchResultViewModel;
         }
      }

      private void UserEditorViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (DomainObjectEditorViewModel.HasUnsavedChanges)
         {
            DomainObjectsSearchViewModel.LockSelection();
         }
         else
         {
            DomainObjectsSearchViewModel.UnlockSelection();
         }

         DomainObjectsManagerViewModel.IsEnabled = DomainObjectEditorViewModel.IsEnabled;
      }

      #endregion
   }
}
