using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.DomainObjectBrowser;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
    public class DomainObjectBrowserViewModel<T> : UIInteractionHost, IDomainObjectBrowserViewModel where T : DomainObject, new()
   {
      #region Fields

      private bool _isExpanded;

      protected bool _loadNestedStructureOnSelection;

      private readonly SemaphoreSlim _domainObjectEditorViewModelsSemaphore = new SemaphoreSlim(1, 1);
      
      protected IDomainObjectViewModelFactory _domainObjectViewModelFactory;      
      protected IDomainObjectsSearchViewModelFactory _domainObjectsSearchViewModelFactory;
      protected IDomainObjectInteractionFactory _domainObjectInteractionFactory;

      protected DomainObjectsSearchViewModel<T> _domainObjectsSearchViewModel;
      protected ObservableCollection<IDomainObjectEditorViewModel> _domainObjectEditorViewModels;
      protected DomainObjectViewModelSelection _selection;
      private bool _isSelectionLocked;
      private bool _isLoadingNestedStructures;
      private bool _isLoadingDomainObjectEditors;
      private DomainObjectViewModel _contextViewModel;

      protected DomainObjectBrowser<T> _domainObjectBrowser;

      #endregion

      #region Properties

      public bool IsExpanded
      {
         get => _isExpanded;
         set => SetProperty(ref _isExpanded, value);
      }

      public DomainObjectsSearchViewModel<T> DomainObjectsSearchViewModel
      {
         get => _domainObjectsSearchViewModel;
         private set => SetProperty(ref _domainObjectsSearchViewModel, value);
      }

      public ObservableCollection<IDomainObjectEditorViewModel> DomainObjectEditorViewModels
      {
         get => _domainObjectEditorViewModels;
         private set => SetProperty(ref _domainObjectEditorViewModels, value);
      }

      public DomainObjectViewModelSelection Selection
      {
         get => _selection;
         private set => SetProperty(ref _selection, value);
      }

      public bool IsSelectionLocked
      {
         get => _isSelectionLocked;
         private set => SetProperty(ref _isSelectionLocked, value);
      }

      public bool IsLoadingNestedStructures
      {
         get => _isLoadingNestedStructures;
         private set => SetProperty(ref _isLoadingNestedStructures, value);
      }

      public bool IsLoadingDomainObjectEditors
      {
         get => _isLoadingDomainObjectEditors;
         private set => SetProperty(ref _isLoadingDomainObjectEditors, value);
      }


      public DomainObjectViewModel ContextViewModel
      {
         get => _contextViewModel;
         set => SetProperty(ref _contextViewModel, value);
      }

      public IDomainObjectEditorViewModel SelectedDomainObjectEditorViewModel => _domainObjectEditorViewModels.FirstOrDefault(vm => vm.DomainObjectViewModel == Selection.SelectedViewModel);

      public bool HasUnsavedChanges => DomainObjectEditorViewModels.Any(vm => vm.HasUnsavedChanges);

      #endregion

      #region Commands

      public AsyncRelayCommand<DomainObjectViewModel?> CreateAsyncCommand { get; private set; }
      public AsyncRelayCommand DeleteSelectionAsyncCommand { get; private set; }
      public AsyncRelayCommand SaveChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelChangesAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectBrowserViewModel(DomainObjectBrowser<T> domainObjectBrowser,
                                          IDomainObjectViewModelFactory domainObjectViewModelFactory,
                                          IUserDialogService userDialogService,
                                          IDomainObjectsSearchViewModelFactory domainObjectsSearchViewModelFactory,
                                          IWindowService windowService,
                                          IDomainObjectInteractionFactory domainObjectInteractionFactory) :
         base(userDialogService, windowService)
      {
         _domainObjectBrowser = domainObjectBrowser;
         _domainObjectViewModelFactory = domainObjectViewModelFactory;
         _domainObjectsSearchViewModelFactory = domainObjectsSearchViewModelFactory;
         _domainObjectInteractionFactory = domainObjectInteractionFactory;

         _loadNestedStructureOnSelection = true;

         CreateAsyncCommand = new AsyncRelayCommand<DomainObjectViewModel?>(CreateAsync, CanCreate);
         DeleteSelectionAsyncCommand = new AsyncRelayCommand(DeleteSelectionAsync, CanDeleteSelection);
         SaveChangesAsyncCommand = new AsyncRelayCommand(SaveChangesAsync, CanSaveChanges);
         CancelChangesAsyncCommand = new AsyncRelayCommand(CancelChangesAsync, CanCancelChanges);

         DomainObjectsSearchViewModel = (DomainObjectsSearchViewModel<T>)_domainObjectsSearchViewModelFactory.CreateDomainObjectSearchViewModel<T>(domainObjectBrowser.DomainObjectSearch, _domainObjectViewModelFactory);
         DomainObjectsSearchViewModel.SearchResultViewModels.CollectionChanged += SearchResultViewModels_CollectionChanged;
         DomainObjectsSearchViewModel.PreviewUnloadSearchResultViewModels += PreviewUnloadSearchResultViewModelsAsync;

         Selection = new DomainObjectViewModelSelection(DomainObjectViewModelSelectionType.Single);
         DomainObjectEditorViewModels = new ObservableCollection<IDomainObjectEditorViewModel>();
      }

      #endregion

      #region Public Methods

      public virtual async Task InitializeAsync()
      {
         await _domainObjectBrowser.InitializeAsync();
         DomainObjectsSearchViewModel.UnloadSearchResultViewModels();
         DomainObjectsSearchViewModel.LoadSearchResultViewModels();
         DomainObjectsSearchViewModel.PageNavigationViewModel.Refresh();
      }

      public virtual async Task SelectSearchResultViewModelAsync(DomainObjectViewModel domainObjectViewModel)
      {
         if (!IsSelectionLocked)
         {
            if (ProceedSelection(domainObjectViewModel))
            {
               UnselectSearchResultViewModel(domainObjectViewModel);

               if (domainObjectViewModel != null)
               {
                  Selection.SelectViewModel(domainObjectViewModel);
               }

               OnPropertyChanged(nameof(SelectedDomainObjectEditorViewModel));
               NotifyCommandsCanExecuteChanged();

               if (_loadNestedStructureOnSelection)
               {
                  IsLoadingNestedStructures = true;

                  await SelectedDomainObjectEditorViewModel.LoadNestedStructuresAsync();

                  IsLoadingNestedStructures = false;
               }
            }
         }
      }

      public void UnselectSearchResultViewModel(DomainObjectViewModel domainObjectViewModel)
      {
         if (!IsSelectionLocked)
         {
            if (domainObjectViewModel != null)
            {
               Selection.UnselectViewModel(domainObjectViewModel);
            }

            OnPropertyChanged(nameof(SelectedDomainObjectEditorViewModel));
            NotifyCommandsCanExecuteChanged();
         }
      }

      public virtual bool CanSaveChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual async Task SaveChangesAsync()
      {
         DomainObjectEditorViewModels.ToList().ForEach(async vm => await vm.SaveChangesAsync());
      }

      public virtual bool CanCancelChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual async Task CancelChangesAsync()
      {
         DomainObjectEditorViewModels.ToList().ForEach(async vm => await vm.CancelChangesAsync());
      }

      public virtual void Dispose()
      {
         UnregisterSearchResultViewModelPropertyChanged();
         DeleteEditorViewModels(DomainObjectsSearchViewModel.SearchResultViewModels);

         DomainObjectsSearchViewModel.SearchResultViewModels.CollectionChanged -= SearchResultViewModels_CollectionChanged;
         DomainObjectsSearchViewModel.PreviewUnloadSearchResultViewModels -= PreviewUnloadSearchResultViewModelsAsync;
         DomainObjectsSearchViewModel.Dispose();
      }

      #endregion

      #region Private Methods

      protected async Task CreateEditorViewModelsAsync(IEnumerable<DomainObjectViewModel> domainObjectViewModels)
      {
         IsLoadingDomainObjectEditors = true;

         foreach (var domainObjectViewModel in domainObjectViewModels)
         {
            var editorViewModel = CreateDomainObjectEditorViewModel(domainObjectViewModel);

            editorViewModel.PropertyChanged += DomainObjectEditorViewModel_PropertyChanged;
            editorViewModel.DomainObjectDeletedEvent += EditorViewModel_DomainObjectDeletedEvent;

            DomainObjectEditorViewModels.Add(editorViewModel);
         }

         SortEditorViewModels();

         IsLoadingDomainObjectEditors = false;
      }

      protected void DeleteEditorViewModels(IEnumerable<DomainObjectViewModel> domainObjectViewModels)
      {
         foreach (var viewModel in domainObjectViewModels)
         {
            Selection.UnselectViewModel(viewModel);

            var editorViewModel = DomainObjectEditorViewModels.FirstOrDefault(editorViewModel => editorViewModel.DomainObjectViewModel.Equals(viewModel));

            if (editorViewModel != null)
            {
               DeleteEditorViewModel(editorViewModel);

               DomainObjectEditorViewModels.Remove(editorViewModel);
            }
         }
      }

      private void DeleteEditorViewModels()
      {
         foreach (var viewModel in DomainObjectEditorViewModels)
         {
            DeleteEditorViewModel(viewModel);
         }

         DomainObjectEditorViewModels.Clear();
      }

      private void DeleteEditorViewModel(IDomainObjectEditorViewModel domainObjectEditorViewModel)
      {
         if (domainObjectEditorViewModel != null)
         {
            domainObjectEditorViewModel.PropertyChanged -= DomainObjectEditorViewModel_PropertyChanged;
            domainObjectEditorViewModel.DomainObjectDeletedEvent -= EditorViewModel_DomainObjectDeletedEvent;
            domainObjectEditorViewModel.Dispose();
         }
      }

      private bool ProceedSelection(DomainObjectViewModel domainObjectViewModel)
      {
         if (Selection.SelectionType == DomainObjectViewModelSelectionType.Single &&
                domainObjectViewModel.Equals(Selection.SelectedViewModel))
         {
            return false;
         }

         return true;
      }

      protected virtual void SortEditorViewModels() { }

      protected virtual bool CanCreate(DomainObjectViewModel? parentViewModel)
      {
         return true;
      }

      protected virtual async Task CreateAsync(DomainObjectViewModel? parentViewModel)
      {
         var domainObject = await _domainObjectBrowser.CreateAsync(parentViewModel?.DomainObject);

         var viewModel = _domainObjectViewModelFactory.CreateViewModel(domainObject);
         _domainObjectsSearchViewModel.AddSearchResultViewModel(viewModel);

         await SelectSearchResultViewModelAsync(viewModel);
      }

      private bool CanDeleteSelection()
      {
         return !Selection.IsSelectionEmpty && !HasUnsavedChanges;
      }

      private async Task DeleteSelectionAsync()
      {
         var confirmation = _userDialogService.Confirm("Confirmez-vous la suppression ?");

         if (confirmation)
         {
            if (Selection.SelectionType == DomainObjectViewModelSelectionType.Single)
            {
               var selectedDomainObjectViewModel = (DomainObjectViewModel)Selection.SelectedViewModel;

               await _domainObjectBrowser.DeleteAsync((T)selectedDomainObjectViewModel.DomainObject);
               _domainObjectsSearchViewModel.RemoveSearchResultViewModel(selectedDomainObjectViewModel);
            }
            else if (Selection.SelectionType == DomainObjectViewModelSelectionType.Multiple)
            {
               foreach (var item in Selection.SelectedViewModels)
               {
                  var selectedDomainObjectViewModel = (DomainObjectViewModel)item;

                  await _domainObjectBrowser.DeleteAsync((T)selectedDomainObjectViewModel.DomainObject);
                  _domainObjectsSearchViewModel.RemoveSearchResultViewModel(selectedDomainObjectViewModel);
               }
            }
         }

         UpdateUnsavedChanges();
      }

      private void EditorViewModel_DomainObjectDeletedEvent(object? sender, DomainObjectViewModel e)
      {
         _domainObjectsSearchViewModel.RemoveSearchResultViewModel(e);
         Selection.UnselectViewModel(e);
         UpdateUnsavedChanges();
      }

      private async void PreviewUnloadSearchResultViewModelsAsync(object? sender, EventArgs e)
      {
         await _domainObjectEditorViewModelsSemaphore.WaitAsync();

         UnselectSearchResultViewModel(Selection.SelectedViewModel);
         UnregisterSearchResultViewModelPropertyChanged();
         DeleteEditorViewModels();

         _domainObjectEditorViewModelsSemaphore.Release();
      }

      private void UnregisterSearchResultViewModelPropertyChanged()
      {
         foreach (var viewModel in DomainObjectsSearchViewModel.SearchResultViewModels)
         {
            viewModel.PropertyChanged -= SearchResultViewModel_PropertyChanged;
         }
      }

      protected virtual IDomainObjectEditorViewModel CreateDomainObjectEditorViewModel(DomainObjectViewModel domainObjectViewModel)
      {
         return _domainObjectInteractionFactory.CreateDomainObjectEditorViewModel<T>(domainObjectViewModel);
      }

      private async void SearchResultViewModels_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
      {
         await _domainObjectEditorViewModelsSemaphore.WaitAsync();

         if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
         {
            foreach (var oldViewModel in e.OldItems)
            {
               if (oldViewModel is DomainObjectViewModel oldDomainObjectViewModel)
               {
                  oldDomainObjectViewModel.PropertyChanged -= SearchResultViewModel_PropertyChanged;
               }
            }

            DeleteEditorViewModels(e.OldItems.Cast<DomainObjectViewModel>());
         }
         else if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
         {
            foreach (var newViewModel in e.NewItems)
            {
               if (newViewModel is DomainObjectViewModel newDomainObjectViewModel)
               {
                  newDomainObjectViewModel.PropertyChanged += SearchResultViewModel_PropertyChanged;
               }
            }

            await CreateEditorViewModelsAsync(e.NewItems.Cast<DomainObjectViewModel>());
         }

         _domainObjectEditorViewModelsSemaphore.Release();
      }

      private void SearchResultViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         NotifyCommandsCanExecuteChanged();
      }

      private void DomainObjectEditorViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(DomainObjectEditorViewModel<T>.HasUnsavedChanges)))
         {
            UpdateUnsavedChanges();
         }
      }

      private void UpdateUnsavedChanges()
      {
         OnPropertyChanged(nameof(HasUnsavedChanges));
         NotifyCommandsCanExecuteChanged();

         if (HasUnsavedChanges)
         {
            LockSelection();
         }
         else
         {
            UnlockSelection();
         }
      }

      private void LockSelection()
      {
         IsSelectionLocked = true;
      }

      private void UnlockSelection()
      {
         IsSelectionLocked = false;
      }

      protected virtual void NotifyCommandsCanExecuteChanged()
      {
         CreateAsyncCommand.NotifyCanExecuteChanged();
         DeleteSelectionAsyncCommand.NotifyCanExecuteChanged();
         SaveChangesAsyncCommand.NotifyCanExecuteChanged();
         CancelChangesAsyncCommand.NotifyCanExecuteChanged();
      }

      #endregion
   }
}
