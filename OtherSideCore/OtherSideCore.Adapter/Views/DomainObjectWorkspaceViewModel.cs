using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OtherSideCore.Adapter.Views
{
   public abstract class DomainObjectWorkspaceViewModel<T> : WorkspaceViewModel where T : DomainObject, new()
   {
      #region Fields

      protected DomainObjectsSearchViewModel<T> _domainObjectsSearchViewModel;
      protected DomainObjectEditorViewModel<T> _domainObjectEditorViewModel;
      protected List<IDomainObjectEditorViewModel> _subDomainObjectEditorViewModels;
      protected DomainObjectManagerViewModel<T> _domainObjectsManagerViewModel;

      #endregion

      #region Properties

      public DomainObjectsSearchViewModel<T> DomainObjectsSearchViewModel
      {
         get => _domainObjectsSearchViewModel;
         set => SetProperty(ref _domainObjectsSearchViewModel, value);
      }

      public DomainObjectEditorViewModel<T> DomainObjectEditorViewModel
      {
         get => _domainObjectEditorViewModel;
         set => SetProperty(ref _domainObjectEditorViewModel, value);
      }

      public List<IDomainObjectEditorViewModel> SubDomainObjectEditorViewModels
      {
         get => _subDomainObjectEditorViewModels;
         set => SetProperty(ref _subDomainObjectEditorViewModels, value);
      }

      public DomainObjectManagerViewModel<T> DomainObjectsManagerViewModel
      {
         get => _domainObjectsManagerViewModel;
         set => SetProperty(ref _domainObjectsManagerViewModel, value);
      }

      public override bool HasUnsavedChanges => DomainObjectEditorViewModel != null ? DomainObjectEditorViewModel.HasUnsavedChanges || SubDomainObjectEditorViewModels.Any(e => e.HasUnsavedChanges) : false;

      #endregion

      #region Commands

      public AsyncRelayCommand SaveChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelChangesAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectWorkspaceViewModel(ILoggerFactory loggerFactory,
                                            IUserContext userContext,
                                            IUserDialogService userDialogService,
                                            IDomainObjectViewModelFactory viewModelFactory,
                                            IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory,
                                            IDomainObjectServiceFactory domainObjectServiceFactory,
                                            IGlobalDataService globalDataService) :
         base(loggerFactory,
              userContext,
              userDialogService,
              globalDataService,
              viewModelFactory,
              domainObjectQueryServiceFactory,
              domainObjectServiceFactory)
      {
         SaveChangesAsyncCommand = new AsyncRelayCommand(SaveChangesAsync, CanSaveChanges);
         CancelChangesAsyncCommand = new AsyncRelayCommand(CancelChangesAsync, CanCancelChanges);

         SubDomainObjectEditorViewModels = new List<IDomainObjectEditorViewModel>();

         DomainObjectsSearchViewModel = new DomainObjectsSearchViewModel<T>(_userContext,
                                                                            _domainObjectQueryServiceFactory.CreateDomainObjectQueryService<T>(),
                                                                            _domainObjectServiceFactory.CreateDomainObjectService<T>(),
                                                                            _viewModelFactory)
         {
            SelectedSearchResultChangedAsync = SelectedSearchResultChangedAsync
         };

         DomainObjectsManagerViewModel = new DomainObjectManagerViewModel<T>(userContext, _domainObjectServiceFactory.CreateDomainObjectService<T>(), userDialogService, viewModelFactory);

         DomainObjectsManagerViewModel.DomainObjectCreated += OnDomainObjectCreated;
         DomainObjectsManagerViewModel.DomainObjectDeleted += OnDomainObjectDeleted;
      }

      #endregion

      #region Public Methods

      public override async Task InitializeAsync(CancellationToken cancellationToken = default)
      {
         await DomainObjectsSearchViewModel.SearchAsync(cancellationToken);
      }

      public override void Dispose()
      {
         DomainObjectsManagerViewModel.DomainObjectCreated -= OnDomainObjectCreated;
         DomainObjectsManagerViewModel.DomainObjectDeleted -= OnDomainObjectDeleted;

         DomainObjectsSearchViewModel.Dispose();
         DomainObjectsManagerViewModel.Dispose();
         UnloadEditorViewModels();
      }

      #endregion

      #region Private Methods

      public virtual bool CanSaveChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual async Task SaveChangesAsync()
      {
         await DomainObjectEditorViewModel.SaveChangesAsync();
         SubDomainObjectEditorViewModels.ToList().ForEach(async vm => await vm.SaveChangesAsync());
      }

      public virtual bool CanCancelChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual async Task CancelChangesAsync()
      {
         await DomainObjectEditorViewModel.CancelChangesAsync();
         SubDomainObjectEditorViewModels.ToList().ForEach(async vm => await vm.CancelChangesAsync());
      }

      protected virtual async Task<DomainObjectEditorViewModel<T>> CreateDomainObjectEditorViewModelAsync(DomainObjectViewModel domainObjectViewModel)
      {
         return new DomainObjectEditorViewModel<T>(_userContext, _domainObjectServiceFactory.CreateDomainObjectService<T>(), DomainObjectsSearchViewModel.SelectedSearchResultViewModel, _userDialogService);
      }

      protected virtual async Task<List<IDomainObjectEditorViewModel>> GetSubDomainObjectEditorViewModelsAsync(DomainObjectViewModel domainObjectViewModel)
      {
         return new List<IDomainObjectEditorViewModel>();
      }

      private void OnDomainObjectDeleted(object? sender, DomainObjectViewModel e)
      {
         DomainObjectsSearchViewModel.RemoveSearchResultViewModel(e);
      }

      private async void OnDomainObjectCreated(object? sender, DomainObjectViewModel e)
      {
         DomainObjectsSearchViewModel.AddSearchResultViewModel(e);
         await DomainObjectsSearchViewModel.SelectSearchResultAsync(e, CancellationToken.None);
      }

      private async Task LoadEditorViewModels(DomainObjectViewModel domainObjectViewModel)
      {
         DomainObjectEditorViewModel = await CreateDomainObjectEditorViewModelAsync(domainObjectViewModel);
         DomainObjectEditorViewModel.PropertyChanged += DomainObjectEditorViewModel_PropertyChanged;

         foreach (var viewModel in await GetSubDomainObjectEditorViewModelsAsync(domainObjectViewModel))
         {
            SubDomainObjectEditorViewModels.Add(viewModel);
            viewModel.PropertyChanged += DomainObjectEditorViewModel_PropertyChanged;
         }
      }

      private void UnloadEditorViewModels()
      {
         if (DomainObjectEditorViewModel != null)
         {
            DomainObjectEditorViewModel.PropertyChanged -= DomainObjectEditorViewModel_PropertyChanged;
            DomainObjectEditorViewModel.Dispose();
            DomainObjectEditorViewModel = null;
         }

         foreach (var viewModel in SubDomainObjectEditorViewModels)
         {
            viewModel.PropertyChanged -= DomainObjectEditorViewModel_PropertyChanged;
         }

         SubDomainObjectEditorViewModels.Clear();

         ReleaseSubDomainObjectsEditorViewModels();
      }

      protected virtual void ReleaseSubDomainObjectsEditorViewModels()
      {

      }

      protected virtual async Task SelectedSearchResultChangedAsync(CancellationToken cancellation)
      {
         UnloadEditorViewModels();

         var selectedDomainObjectViewModel = DomainObjectsSearchViewModel.SelectedSearchResultViewModel;

         await _domainObjectServiceFactory.CreateDomainObjectService<T>().LoadTrackingInfosAsync((T)selectedDomainObjectViewModel.DomainObject);

         if (selectedDomainObjectViewModel != null)
         {
            LoadEditorViewModels(selectedDomainObjectViewModel);

            DomainObjectsManagerViewModel.SelectedDomainObjectViewModel = DomainObjectsSearchViewModel.SelectedSearchResultViewModel;
         }
      }

      private void DomainObjectEditorViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         var domainObjectEditorViewModel = (IDomainObjectEditorViewModel)sender;

         if (e.PropertyName.Equals(nameof(IDomainObjectEditorViewModel.HasUnsavedChanges)))
         {
            OnPropertyChanged(nameof(HasUnsavedChanges));
            NotifyCommandsCanExecuteChanged();

            if (HasUnsavedChanges)
            {
               DomainObjectsSearchViewModel.LockSelection();
            }
            else
            {
               DomainObjectsSearchViewModel.UnlockSelection();
            }
         }

         DomainObjectsManagerViewModel.IsEnabled = SubDomainObjectEditorViewModels.All(vm => vm.IsEnabled) && DomainObjectEditorViewModel.IsEnabled;
      }

      protected override void NotifyCommandsCanExecuteChanged()
      {
         base.NotifyCommandsCanExecuteChanged();

         SaveChangesAsyncCommand.NotifyCanExecuteChanged();
         CancelChangesAsyncCommand.NotifyCanExecuteChanged();
      }

      #endregion
   }
}
