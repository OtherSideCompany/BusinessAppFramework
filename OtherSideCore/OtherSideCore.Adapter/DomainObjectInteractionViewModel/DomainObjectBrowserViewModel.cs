using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Application.Browser;
using OtherSideCore.Application.Factories;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
    public class DomainObjectBrowserViewModel<T> : UIInteractionHost, IDomainObjectBrowserViewModel where T : DomainObject, new()
   {
      #region Fields

      private bool _isExpanded;

      protected bool _loadNestedStructureOnSelection;

      protected IDomainObjectsSearchViewModelFactory _domainObjectsSearchViewModelFactory;
      protected IDomainObjectInteractionFactory _domainObjectInteractionFactory;

      protected DomainObjectsSearchViewModel<T> _domainObjectsSearchViewModel;

      private IDomainObjectEditorViewModel _selectedDomainObjectEditorViewModel;

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

      public DomainObjectsSearchViewModel<T> DomainObjectsSearchViewModel
      {
         get => _domainObjectsSearchViewModel;
         private set => SetProperty(ref _domainObjectsSearchViewModel, value);
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

      public bool HasUnsavedChanges => SelectedDomainObjectEditorViewModel == null ? false : SelectedDomainObjectEditorViewModel.HasUnsavedChanges;

      #endregion

      #region Commands

      public AsyncRelayCommand<DomainObjectViewModel?> CreateAsyncCommand { get; private set; }
      public AsyncRelayCommand SaveChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelChangesAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectBrowserViewModel(DomainObjectBrowser<T> domainObjectBrowser,                                          
                                          IUserDialogService userDialogService,                                          
                                          IDomainObjectsSearchViewModelFactory domainObjectsSearchViewModelFactory,
                                          IDomainObjectSearchResultViewModelFactory domainObjectSearchResultViewModelFactory,
                                          IDomainObjectSearchResultFactory domainObjectSearchResultFactory,
                                          IWindowService windowService,
                                          IDomainObjectInteractionFactory domainObjectInteractionFactory) :
         base(userDialogService, windowService)
      {
         _domainObjectBrowser = domainObjectBrowser;
         _domainObjectsSearchViewModelFactory = domainObjectsSearchViewModelFactory;
         _domainObjectInteractionFactory = domainObjectInteractionFactory;

         _loadNestedStructureOnSelection = true;

         CreateAsyncCommand = new AsyncRelayCommand<DomainObjectViewModel?>(CreateAsync, CanCreate);
         SaveChangesAsyncCommand = new AsyncRelayCommand(SaveChangesAsync, CanSaveChanges);
         CancelChangesAsyncCommand = new AsyncRelayCommand(CancelChangesAsync, CanCancelChanges);

         DomainObjectsSearchViewModel = (DomainObjectsSearchViewModel<T>)_domainObjectsSearchViewModelFactory.CreateDomainObjectSearchViewModel<T>(domainObjectBrowser.DomainObjectSearch, domainObjectSearchResultViewModelFactory, domainObjectSearchResultFactory);
         DomainObjectsSearchViewModel.PreviewUnloadSearchResultViewModels += PreviewUnloadSearchResultViewModelsAsync;

         Selection = new Selection(SelectionType.Single);
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

            DeleteSelectedEditorViewModel();

            NotifyCommandsCanExecuteChanged();
         }
      }

      public virtual bool CanSaveChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual async Task SaveChangesAsync()
      {
         await SelectedDomainObjectEditorViewModel?.SaveChangesAsync();

         _domainObjectsSearchViewModel.ReloadSearchResultAsync(((DomainObjectSearchResultViewModel)Selection.SelectedItem).DomainObjectSearchResult.DomainObjectId);
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
         DeleteSelectedEditorViewModel();

         DomainObjectsSearchViewModel.PreviewUnloadSearchResultViewModels -= PreviewUnloadSearchResultViewModelsAsync;
         DomainObjectsSearchViewModel.Dispose();
      }

      #endregion

      #region Private Methods

      private async Task CreateEditorViewModelsAsync(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel)
      {
         IsLoadingDomainObjectEditor = true;

         var editorViewModel = await CreateDomainObjectEditorViewModelAsync(domainObjectSearchResultViewModel);

         editorViewModel.PropertyChanged += DomainObjectEditorViewModel_PropertyChanged;
         editorViewModel.DomainObjectDeletedEvent += EditorViewModel_DomainObjectDeletedEvent;

         SelectedDomainObjectEditorViewModel = editorViewModel;

         IsLoadingDomainObjectEditor = false;
      }

      private void DeleteSelectedEditorViewModel()
      {
         if (SelectedDomainObjectEditorViewModel != null)
         {
            SelectedDomainObjectEditorViewModel.Dispose();

            SelectedDomainObjectEditorViewModel.PropertyChanged -= DomainObjectEditorViewModel_PropertyChanged;
            SelectedDomainObjectEditorViewModel.DomainObjectDeletedEvent -= EditorViewModel_DomainObjectDeletedEvent;

            SelectedDomainObjectEditorViewModel = null;
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

      protected virtual async Task CreateAsync(DomainObjectViewModel? parentViewModel)
      {
         var domainObject = await _domainObjectBrowser.CreateAsync(parentViewModel?.DomainObject);
         var searchResultViewModel = _domainObjectsSearchViewModel.AddSearchResultViewModel(domainObject.Id);

         await SelectSearchResultViewModelAsync(searchResultViewModel);
      }

      private void EditorViewModel_DomainObjectDeletedEvent(object? sender, int domainObjectId)
      {
         _domainObjectsSearchViewModel.RemoveSearchResultViewModel(domainObjectId);
         Selection.ClearSelection();
         UpdateUnsavedChanges();
      }

      private async void PreviewUnloadSearchResultViewModelsAsync(object? sender, EventArgs e)
      {
         DeleteSelectedEditorViewModel();
      }

      protected async virtual Task<IDomainObjectEditorViewModel> CreateDomainObjectEditorViewModelAsync(DomainObjectSearchResultViewModel domainObjectSearchResultViewModel)
      {
         return await _domainObjectInteractionFactory.CreateDomainObjectEditorViewModelAsync<T>(domainObjectSearchResultViewModel.DomainObjectSearchResult.DomainObjectId);
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
         SaveChangesAsyncCommand.NotifyCanExecuteChanged();
         CancelChangesAsyncCommand.NotifyCanExecuteChanged();
      }

      #endregion
   }
}
