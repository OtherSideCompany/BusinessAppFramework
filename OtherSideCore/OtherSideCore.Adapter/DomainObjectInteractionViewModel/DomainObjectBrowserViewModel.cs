using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Application.Browser;
using OtherSideCore.Application.Factories;
using OtherSideCore.Domain.DomainObjects;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public class DomainObjectBrowserViewModel<T> : ObservableObject, IDomainObjectBrowserViewModel, ISavable where T : DomainObject, new()
   {
      #region Fields

      private bool _isExpanded;

      protected bool _loadNestedStructureOnSelection;

      protected IDomainObjectsSearchViewModelFactory _domainObjectsSearchViewModelFactory;
      protected IDomainObjectInteractionService _domainObjectInteractionService;
      protected IDomainObjectServiceFactory _domainObjectServiceFactory;

      protected IDomainObjectSearchViewModel _domainObjectSearchViewModel;

      private IDomainObjectEditorViewModel _selectedDomainObjectEditorViewModel;
      private IDomainObjectEditorViewModel? _selectedDomainObjectDetailsEditorViewModel;

      protected Selection _selection;
      private bool _isSelectionLocked;
      private bool _isLoadingEditor;
      private bool _isLoadingDomainObjectEditor;
      private DomainObjectViewModel _contextViewModel;

      protected DomainObjectBrowser<T> _domainObjectBrowser;

      #endregion

      #region Properties

      public bool IsExpanded
      {
         get => _isExpanded;
         set => SetProperty(ref _isExpanded, value);
      }

      public IDomainObjectSearchViewModel DomainObjectSearchViewModel
      {
         get => _domainObjectSearchViewModel;
         private set => SetProperty(ref _domainObjectSearchViewModel, value);
      }

      public Selection Selection
      {
         get => _selection;
         private set => SetProperty(ref _selection, value);
      }

      public bool IsSelectionLocked
      {
         get => _isSelectionLocked;
         private set => SetProperty(ref _isSelectionLocked, value);
      }

      public bool IsLoadingEditor
      {
         get => _isLoadingEditor;
         private set => SetProperty(ref _isLoadingEditor, value);
      }

      public bool IsLoadingDomainObjectEditor
      {
         get => _isLoadingDomainObjectEditor;
         private set => SetProperty(ref _isLoadingDomainObjectEditor, value);
      }

      public DomainObjectViewModel ContextViewModel
      {
         get => _contextViewModel;
         set => SetProperty(ref _contextViewModel, value);
      }

      public IDomainObjectEditorViewModel SelectedDomainObjectEditorViewModel
      {
         get => _selectedDomainObjectEditorViewModel;
         set => SetProperty(ref _selectedDomainObjectEditorViewModel, value);
      }

      public IDomainObjectEditorViewModel? SelectedDomainObjectDetailsEditorViewModel
      {
         get => _selectedDomainObjectDetailsEditorViewModel;
         set => SetProperty(ref _selectedDomainObjectDetailsEditorViewModel, value);
      }

      public bool HasUnsavedChanges => SelectedDomainObjectEditorViewModel == null ? false : SelectedDomainObjectEditorViewModel.HasUnsavedChanges;

      #endregion

      #region Commands

      public AsyncRelayCommand<DomainObjectViewModel?> CreateAsyncCommand { get; private set; }
      public AsyncRelayCommand SaveChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand<DomainObjectSearchResultViewModel> ShowDomainObjectDetailsEditorAsyncCommand { get; private set; }
      public AsyncRelayCommand<SearchParameters> SearchCommandAsync { get; private set; }
      public AsyncRelayCommand<PaginatedSearchParameters> PaginatedSearchCommandAsync { get; private set; }
      public RelayCommand CancelSearchCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectBrowserViewModel(DomainObjectBrowser<T> domainObjectBrowser,
                                          IDomainObjectsSearchViewModelFactory domainObjectsSearchViewModelFactory,
                                          IDomainObjectSearchResultViewModelFactory domainObjectSearchResultViewModelFactory,
                                          IDomainObjectInteractionService domainObjectInteractionFactory,
                                          IDomainObjectServiceFactory domainObjectServiceFactory)
      {
         _domainObjectBrowser = domainObjectBrowser;
         _domainObjectsSearchViewModelFactory = domainObjectsSearchViewModelFactory;
         _domainObjectInteractionService = domainObjectInteractionFactory;
         _domainObjectServiceFactory = domainObjectServiceFactory;

         _loadNestedStructureOnSelection = true;

         CreateAsyncCommand = new AsyncRelayCommand<DomainObjectViewModel?>(CreateAsync, CanCreate);
         SaveChangesAsyncCommand = new AsyncRelayCommand(SaveChangesAsync, CanSaveChanges);
         CancelChangesAsyncCommand = new AsyncRelayCommand(CancelChangesAsync, CanCancelChanges);
         ShowDomainObjectDetailsEditorAsyncCommand = new AsyncRelayCommand<DomainObjectSearchResultViewModel>(ShowDomainObjectDetailsEditorAsync, CanShowDomainObjectDetailsEditor);
         SearchCommandAsync = new AsyncRelayCommand<SearchParameters>(SearchAsync);
         PaginatedSearchCommandAsync = new AsyncRelayCommand<PaginatedSearchParameters>(PaginatedSearchAsync);
         CancelSearchCommand = new RelayCommand(CancelSearch);

         DomainObjectSearchViewModel = (DomainObjectsSearchViewModel<T>)_domainObjectsSearchViewModelFactory.CreateDomainObjectSearchViewModel<T>(domainObjectBrowser.DomainObjectSearch, domainObjectSearchResultViewModelFactory);
         ((DomainObjectsSearchViewModel<T>)DomainObjectSearchViewModel).PreviewUnloadSearchResultViewModels += PreviewUnloadSearchResultViewModelsAsync;

         Selection = new Selection(SelectionType.Single);
      }

      #endregion

      #region Public Methods

      public virtual async Task SearchAsync(SearchParameters parameters)
      {
         await DomainObjectSearchViewModel.SearchAsync(parameters);
      }

      public virtual async Task PaginatedSearchAsync(PaginatedSearchParameters parameters)
      {
         await DomainObjectSearchViewModel.PaginatedSearchAsync(parameters);
      }

      public void CancelSearch()
      {
         DomainObjectSearchViewModel.CancelSearch();
      }

      public virtual async Task InitializeAsync()
      {
         await _domainObjectBrowser.InitializeAsync();
         DomainObjectSearchViewModel.UnloadSearchResultViewModels();
         DomainObjectSearchViewModel.LoadSearchResultViewModels();
         ((DomainObjectsSearchViewModel<T>)DomainObjectSearchViewModel).PageNavigationViewModel.Refresh();
      }

      public virtual async Task SelectSearchResultViewModelAsync(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel)
      {
         if (!IsSelectionLocked)
         {
            if (ProceedSelection(domainObjectSearchResultViewModel))
            {
               UnselectSearchResultViewModel();

               if (domainObjectSearchResultViewModel != null)
               {
                  Selection.Select(domainObjectSearchResultViewModel);

                  IsLoadingEditor = true;

                  await CreateEditorViewModelsAsync(domainObjectSearchResultViewModel);

                  if (_loadNestedStructureOnSelection)
                  {
                     await SelectedDomainObjectEditorViewModel.LoadNestedStructuresAsync();
                  }

                  IsLoadingEditor = false;
               }

               NotifyCommandsCanExecuteChanged();

            }
         }
      }

      public void UnselectSearchResultViewModel()
      {
         if (!IsSelectionLocked)
         {
            Selection.ClearSelection();

            DeleteSelectedEditorsViewModel();

            NotifyCommandsCanExecuteChanged();
         }
      }

      public bool CanShowDomainObjectDetailsEditor(DomainObjectSearchResultViewModel? obj)
      {
         return !HasUnsavedChanges;
      }

      public virtual async Task ShowDomainObjectDetailsEditorAsync(DomainObjectSearchResultViewModel? domainObjectSearchResultViewModel)
      {
         if (domainObjectSearchResultViewModel != null)
         {
            await ShowDomainObjectDetailsEditorAsync(typeof(T), domainObjectSearchResultViewModel.DomainObjectSearchResult.DomainObjectId);
         }
      }

      public virtual bool CanSaveChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual async Task SaveChangesAsync()
      {
         await SelectedDomainObjectEditorViewModel?.SaveChangesAsync();
      }

      public virtual bool CanCancelChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual async Task CancelChangesAsync()
      {
         await SelectedDomainObjectEditorViewModel?.CancelChangesAsync();
      }

      public virtual void Dispose()
      {
         DeleteSelectedEditorsViewModel();

         ((DomainObjectsSearchViewModel<T>)DomainObjectSearchViewModel).PreviewUnloadSearchResultViewModels -= PreviewUnloadSearchResultViewModelsAsync;
         DomainObjectSearchViewModel.Dispose();
      }

      #endregion

      #region Private Methods

      protected async Task ShowDomainObjectDetailsEditorAsync(Type type, int domainObjectId)
      {
         SelectedDomainObjectDetailsEditorViewModel = await _domainObjectInteractionService.DisplayDomainObjectDetailsEditorViewAsync(domainObjectId, type, DisplayType.Modal);

         if (SelectedDomainObjectDetailsEditorViewModel != null)
         {
            SelectedDomainObjectDetailsEditorViewModel.DomainObjectSavedEvent += DomainObjectDetailsEditorViewModel_DomainObjectSavedEvent;
            SelectedDomainObjectEditorViewModel.DomainObjectDeletedEvent += SelectedEditorViewModel_DomainObjectDeletedEvent;

            foreach (var nestedTreeViewModel in SelectedDomainObjectDetailsEditorViewModel.NestedDomainObjectTreeViewModels)
            {
               nestedTreeViewModel.TreeModified += DomainObjectDetailsEditorViewModel_NestedTreeViewModelTreeModifiedAsync;
            }
         }
      }  

      private async Task CreateEditorViewModelsAsync(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel)
      {
         IsLoadingDomainObjectEditor = true;

         var editorViewModel = await CreateDomainObjectEditorViewModelAsync(domainObjectSearchResultViewModel);
         await editorViewModel.LoadDomainObjetReferencesAsync();

         editorViewModel.PropertyChanged += SelectedDomainObjectEditorViewModel_PropertyChanged;
         editorViewModel.DomainObjectDeletedEvent += SelectedEditorViewModel_DomainObjectDeletedEvent;
         editorViewModel.DomainObjectSavedEvent += DomainObjectEditorViewModel_DomainObjectSavedEvent;
         editorViewModel.DomainObjectReferencesModified += SelectedEditorViewModel_DomainObjectReferencesModified;

         SelectedDomainObjectEditorViewModel = editorViewModel;

         IsLoadingDomainObjectEditor = false;
      }

      protected virtual void DeleteSelectedEditorsViewModel()
      {
         DeleteSelectedEditorViewModel();
         DeleteSelectedDetailEditorViewModel();
      }

      private void DeleteSelectedEditorViewModel()
      {
         if (SelectedDomainObjectEditorViewModel != null)
         {
            SelectedDomainObjectEditorViewModel.Dispose();

            SelectedDomainObjectEditorViewModel.PropertyChanged -= SelectedDomainObjectEditorViewModel_PropertyChanged;
            SelectedDomainObjectEditorViewModel.DomainObjectSavedEvent -= DomainObjectEditorViewModel_DomainObjectSavedEvent;
            SelectedDomainObjectEditorViewModel.DomainObjectDeletedEvent -= SelectedEditorViewModel_DomainObjectDeletedEvent;
            SelectedDomainObjectEditorViewModel.DomainObjectReferencesModified -= SelectedEditorViewModel_DomainObjectReferencesModified;

            SelectedDomainObjectEditorViewModel = null;
         }         
      }

      private void DeleteSelectedDetailEditorViewModel()
      {
         if (SelectedDomainObjectDetailsEditorViewModel != null)
         {
            foreach (var nestedTreeViewModel in SelectedDomainObjectDetailsEditorViewModel.NestedDomainObjectTreeViewModels)
            {
               nestedTreeViewModel.TreeModified -= DomainObjectDetailsEditorViewModel_NestedTreeViewModelTreeModifiedAsync;
            }

            SelectedDomainObjectDetailsEditorViewModel.DomainObjectSavedEvent -= DomainObjectDetailsEditorViewModel_DomainObjectSavedEvent;
            SelectedDomainObjectDetailsEditorViewModel.DomainObjectDeletedEvent -= SelectedEditorViewModel_DomainObjectDeletedEvent;
            SelectedDomainObjectDetailsEditorViewModel.Dispose();
            SelectedDomainObjectDetailsEditorViewModel = null;
         }
      }      

      private bool ProceedSelection(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel)
      {
         if (Selection.SelectionType == SelectionType.Single &&
                domainObjectSearchResultViewModel.Equals(Selection.SelectedItem))
         {
            return false;
         }

         return true;
      }

      protected virtual bool CanCreate(DomainObjectViewModel? parentViewModel)
      {
         return true;
      }

      protected virtual async Task<T> CreateAsync(DomainObjectViewModel? parentViewModel)
      {
         var domainObject = await _domainObjectBrowser.CreateAsync(parentViewModel?.DomainObject);
         var searchResultViewModel = await DomainObjectSearchViewModel.InsertSearchResultViewModelAsync(domainObject.Id, 0);

         await SelectSearchResultViewModelAsync(searchResultViewModel);

         return domainObject;
      }

      protected virtual void SelectedEditorViewModel_DomainObjectDeletedEvent(object? sender, int domainObjectId)
      {
         DomainObjectSearchViewModel.RemoveSearchResultViewModel(domainObjectId);
         Selection.ClearSelection();
         DeleteSelectedEditorsViewModel();
         UpdateUnsavedChanges();
      }

      private void SelectedEditorViewModel_DomainObjectReferencesModified(object? sender, EventArgs e)
      {
         DomainObjectSearchViewModel.ReloadSearchResultAsync(((DomainObjectSearchResultViewModel)Selection.SelectedItem).DomainObjectSearchResult.DomainObjectId);
      }

      private void PreviewUnloadSearchResultViewModelsAsync(object? sender, EventArgs e)
      {
         DeleteSelectedEditorsViewModel();
      }

      protected async virtual Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel)
      {
         return await _domainObjectInteractionService.CreateDomainObjectEditorViewModelAsync<T>(domainObjectSearchResultViewModel.DomainObjectSearchResult.DomainObjectId);
      }

      protected virtual void SelectedDomainObjectEditorViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
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
         SaveChangesAsyncCommand.NotifyCanExecuteChanged();
         CancelChangesAsyncCommand.NotifyCanExecuteChanged();
         ShowDomainObjectDetailsEditorAsyncCommand.NotifyCanExecuteChanged();
      }

      protected virtual void DomainObjectEditorViewModel_DomainObjectSavedEvent(object? sender, int e)
      {
         DomainObjectSearchViewModel.ReloadSearchResultAsync(((DomainObjectSearchResultViewModel)Selection.SelectedItem).DomainObjectSearchResult.DomainObjectId);
      }

      protected virtual void DomainObjectDetailsEditorViewModel_DomainObjectSavedEvent(object? sender, int e)
      {         
         DomainObjectSearchViewModel.ReloadSearchResultAsync(((DomainObjectSearchResultViewModel)Selection.SelectedItem).DomainObjectSearchResult.DomainObjectId);
         // reload simple editor values
      }

      private async void DomainObjectDetailsEditorViewModel_NestedTreeViewModelTreeModifiedAsync(object? sender, EventArgs e)
      {
         var domainObjectService = _domainObjectServiceFactory.CreateDomainObjectService<T>();

         var ids = DomainObjectSearchViewModel.SearchResultViewModels.Select(vm => vm.DomainObjectSearchResult.DomainObjectId).ToList();

         foreach (var id in ids)
         {
            if (!await domainObjectService.ExistsAsync(id))
            {
               if (SelectedDomainObjectEditorViewModel != null && SelectedDomainObjectEditorViewModel.DomainObjectViewModel.DomainObject.Id == id)
               {
                  DeleteSelectedEditorViewModel();
               }

               DomainObjectSearchViewModel.RemoveSearchResultViewModel(id);
            }
         }
      }

      #endregion
   }
}
