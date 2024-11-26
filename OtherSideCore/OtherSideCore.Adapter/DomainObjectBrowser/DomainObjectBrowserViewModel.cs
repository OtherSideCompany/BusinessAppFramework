using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.DomainObjectBrowser;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectBrowser
{
    public class DomainObjectBrowserViewModel<T> : UIInteractionHost, IDomainObjectBrowserViewModel where T : DomainObject, new()
   {
      #region Fields

      private bool _isExpanded;      

      private readonly SemaphoreSlim _domainObjectEditorViewModelsSemaphore = new SemaphoreSlim(1, 1);
      
      protected IDomainObjectViewModelFactory _domainObjectViewModelFactory;      
      protected IDomainObjectsSearchViewModelFactory _domainObjectsSearchViewModelFactory;
      protected IDomainObjectInteractionFactory _domainObjectInteractionFactory;

      protected DomainObjectsSearchViewModel<T> _domainObjectsSearchViewModel;
      protected ObservableCollection<IDomainObjectEditorViewModel> _domainObjectEditorViewModels;
      protected ObservableCollection<IDomainObjectBrowserViewModel> _nestedDomainObjectBrowserViewModels;
      protected DomainObjectViewModelSelection _selection;
      private bool _isSelectionLocked;
      private bool _isLoadingNestedBrowsers;
      private bool _isLoadingDomainObjectEditors;
      private DomainObjectViewModel _parentContextViewModel;

      protected DomainObjectBrowser<T> _domainObjectBrowser;

      private IEnumerable<IDomainObjectBrowserViewModel> _inlineNestedDomainObjectBrowserViewModels
      {
         get
         {
            foreach (var domainObjectBrowserViewModel in _nestedDomainObjectBrowserViewModels)
            {
               yield return domainObjectBrowserViewModel;

               if (domainObjectBrowserViewModel.NestedDomainObjectBrowserViewModels.Any())
               {
                  foreach (var nestedDomainObjectBrowserViewModel in domainObjectBrowserViewModel.InlineNestedDomainObjectBrowserViewModels)
                  {
                     yield return nestedDomainObjectBrowserViewModel;
                  }
               }
            }
         }
      }

      #endregion

      #region Properties

      public ObservableCollection<IDomainObjectBrowserViewModel> NestedDomainObjectBrowserViewModels => _nestedDomainObjectBrowserViewModels;

      public IEnumerable<IDomainObjectBrowserViewModel> InlineNestedDomainObjectBrowserViewModels => _inlineNestedDomainObjectBrowserViewModels;

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

      public bool IsLoadingNestedBrowsers
      {
         get => _isLoadingNestedBrowsers;
         private set => SetProperty(ref _isLoadingNestedBrowsers, value);
      }

      public bool IsLoadingDomainObjectEditors
      {
         get => _isLoadingDomainObjectEditors;
         private set => SetProperty(ref _isLoadingDomainObjectEditors, value);
      }


      public DomainObjectViewModel ParentContextViewModel
      {
         get => _parentContextViewModel;
         set => SetProperty(ref _parentContextViewModel, value);
      }

      public IDomainObjectEditorViewModel SelectedDomainObjectEditorViewModel => _domainObjectEditorViewModels.FirstOrDefault(vm => vm.DomainObjectViewModel == Selection.SelectedViewModel);

      public bool HasUnsavedChanges => DomainObjectEditorViewModels.Any(vm => vm.HasUnsavedChanges) || _nestedDomainObjectBrowserViewModels.Any(vm => vm.HasUnsavedChanges);

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

         CreateAsyncCommand = new AsyncRelayCommand<DomainObjectViewModel?>(CreateAsync, CanCreate);
         DeleteSelectionAsyncCommand = new AsyncRelayCommand(DeleteSelectionAsync, CanDeleteSelection);
         SaveChangesAsyncCommand = new AsyncRelayCommand(SaveChangesAsync, CanSaveChanges);
         CancelChangesAsyncCommand = new AsyncRelayCommand(CancelChangesAsync, CanCancelChanges);

         DomainObjectsSearchViewModel = (DomainObjectsSearchViewModel<T>)_domainObjectsSearchViewModelFactory.CreateDomainObjectSearchViewModel<T>(domainObjectBrowser.DomainObjectSearch, _domainObjectViewModelFactory);
         DomainObjectsSearchViewModel.SearchResultViewModels.CollectionChanged += SearchResultViewModels_CollectionChanged;
         DomainObjectsSearchViewModel.PreviewUnloadSearchResultViewModels += PreviewUnloadSearchResultViewModelsAsync;

         Selection = new DomainObjectViewModelSelection(DomainObjectViewModelSelectionType.Single);
         DomainObjectEditorViewModels = new ObservableCollection<IDomainObjectEditorViewModel>();
         _nestedDomainObjectBrowserViewModels = new ObservableCollection<IDomainObjectBrowserViewModel>();
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

      public async Task SelectSearchResultViewModelAsync(DomainObjectViewModel domainObjectViewModel)
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

               IsLoadingNestedBrowsers = true;

               await LoadNestedBrowsersAsync();

               foreach (var nestedBrowserViewModel in InlineNestedDomainObjectBrowserViewModels)
               {
                  await nestedBrowserViewModel.LoadNestedBrowsersAsync();
                  nestedBrowserViewModel.PropertyChanged += NestedDomainObjectBrowserViewModel_PropertyChanged;
               }

               IsLoadingNestedBrowsers = false;
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

            foreach (var nestedBrowserViewModel in InlineNestedDomainObjectBrowserViewModels)
            {
               nestedBrowserViewModel.PropertyChanged -= NestedDomainObjectBrowserViewModel_PropertyChanged;
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
         InlineNestedDomainObjectBrowserViewModels.ToList().ForEach(async vm => await vm.SaveChangesAsync());
      }

      public virtual bool CanCancelChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual async Task CancelChangesAsync()
      {
         DomainObjectEditorViewModels.ToList().ForEach(async vm => await vm.CancelChangesAsync());
         InlineNestedDomainObjectBrowserViewModels.ToList().ForEach(async vm => await vm.CancelChangesAsync());
      }

      public async Task LoadEditorViewModelsAsync(IEnumerable<DomainObjectViewModel> domainObjectViewModels)
      {
         IsLoadingDomainObjectEditors = true;

         foreach (var domainObjectViewModel in domainObjectViewModels)
         {
            var editorViewModel = CreateDomainObjectEditorViewModel(domainObjectViewModel);

            await editorViewModel.LoadNestedBrowsersAsync();

            editorViewModel.PropertyChanged += DomainObjectEditorViewModel_PropertyChanged;
            editorViewModel.DomainObjectDeletedEvent += EditorViewModel_DomainObjectDeletedEvent;

            DomainObjectEditorViewModels.Add(editorViewModel);
         }

         SortEditorViewModels();

         IsLoadingDomainObjectEditors = false;
      }

      public void UnloadEditorViewModels(IEnumerable<DomainObjectViewModel> domainObjectViewModels)
      {
         foreach (var viewModel in domainObjectViewModels)
         {
            Selection.UnselectViewModel(viewModel);

            var editorViewModel = DomainObjectEditorViewModels.FirstOrDefault(editorViewModel => editorViewModel.DomainObjectViewModel.Equals(viewModel));            

            if (editorViewModel != null)
            {
               UnloadEditorViewModel(editorViewModel);

               DomainObjectEditorViewModels.Remove(editorViewModel);
            }            
         }
      }      

      public virtual async Task LoadNestedBrowsersAsync()
      {

      }

      public virtual void Dispose()
      {
         UnregisterSearchResultViewModelPropertyChanged();
         UnloadEditorViewModels(DomainObjectsSearchViewModel.SearchResultViewModels);

         DomainObjectsSearchViewModel.SearchResultViewModels.CollectionChanged -= SearchResultViewModels_CollectionChanged;
         DomainObjectsSearchViewModel.PreviewUnloadSearchResultViewModels -= PreviewUnloadSearchResultViewModelsAsync;
         DomainObjectsSearchViewModel.Dispose();

         foreach (var viewModel in InlineNestedDomainObjectBrowserViewModels)
         {
            viewModel.PropertyChanged -= NestedDomainObjectBrowserViewModel_PropertyChanged;
         }

         _nestedDomainObjectBrowserViewModels.ToList().ForEach(vm => vm.Dispose());
         _nestedDomainObjectBrowserViewModels.Clear();
      }

      #endregion

      #region Private Methods

      private void UnloadEditorViewModels()
      {
         foreach (var viewModel in DomainObjectEditorViewModels)
         {
            UnloadEditorViewModel(viewModel);
         }

         DomainObjectEditorViewModels.Clear();
      }

      private void UnloadEditorViewModel(IDomainObjectEditorViewModel domainObjectEditorViewModel)
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
         UnloadEditorViewModels();

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

            UnloadEditorViewModels(e.OldItems.Cast<DomainObjectViewModel>());
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

            await LoadEditorViewModelsAsync(e.NewItems.Cast<DomainObjectViewModel>());
         }

         _domainObjectEditorViewModelsSemaphore.Release();
      }

      private void SearchResultViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         NotifyCommandsCanExecuteChanged();
      }

      protected void NestedDomainObjectBrowserViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(DomainObjectBrowser<T>.HasUnsavedChanges)))
         {
            UpdateUnsavedChanges();
         }
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
