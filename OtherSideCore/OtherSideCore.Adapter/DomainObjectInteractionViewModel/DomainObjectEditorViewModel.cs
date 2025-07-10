using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.Attributes;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.Services;
using OtherSideCore.Adapter.Workflows;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public class DomainObjectEditorViewModel<T> : ObservableObject, IDomainObjectEditorViewModel, ISavable, IDomainObjectInteractionHost where T : DomainObject, new()
   {
      #region Fields

      protected DomainObjectViewModel _domainObjectViewModel;
      protected IDomainObjectService<T> _domainObjectService;

      protected DomainObjectEditorViewModelDependencies _domainObjectEditorViewModelDependencies;
      //private ObservableCollection<DomainObjectReferenceSelectorViewModel> _domainObjectReferenceSelectorViewModels;

      protected ObservableCollection<DomainObjectTreeViewModel> _nestedDomainObjectTreeViewModels;      
      protected ObservableCollection<ProcessWorkflowViewModel> _processWorkflowViewModels;

      //protected ObservableCollection<DomainObjectReferenceViewModel> _domainObjectReferenceViewModels;
      protected IDomainObjectReferencesEditorViewModel _domainObjectReferencesEditorViewModel;

      private bool _isEnabled;
      private bool _hasUnsavedChanges;
      private bool _isReadOnly;

      private readonly SemaphoreSlim _loadDomainObjectReferencesLock = new(1, 1);

      protected StringKey _domainObjectEditorKey;

      #endregion

      #region Properties


      public DomainObjectViewModel DomainObjectViewModel => _domainObjectViewModel;
      public DomainObjectEditorViewModelDependencies DomainObjectEditorViewModelDependencies => _domainObjectEditorViewModelDependencies;

      //public ObservableCollection<DomainObjectReferenceViewModel> DomainObjectReferenceViewModels => _domainObjectReferenceViewModels;

      public ObservableCollection<DomainObjectTreeViewModel> NestedDomainObjectTreeViewModels => _nestedDomainObjectTreeViewModels;

      /*public ObservableCollection<DomainObjectReferenceSelectorViewModel> DomainObjectReferenceSelectorViewModels
      {
         get => _domainObjectReferenceSelectorViewModels;
         private set => SetProperty(ref _domainObjectReferenceSelectorViewModels, value);
      }*/

      public bool IsEnabled
      {
         get => _isEnabled;
         set => SetProperty(ref _isEnabled, value);
      }

      public IDomainObjectReferencesEditorViewModel DomainObjectReferencesEditorViewModel
      {
         get => _domainObjectReferencesEditorViewModel;
         set => SetProperty(ref _domainObjectReferencesEditorViewModel, value);
      }

      public bool HasUnsavedChanges
      {
         get => _hasUnsavedChanges;
         set => SetProperty(ref _hasUnsavedChanges, value);
      }

      public bool IsReadOnly
      {
         get => _isReadOnly;
         set => SetProperty(ref _isReadOnly, value);
      }

      public IDomainObjectInteractionService DomainObjectInteractionService => _domainObjectEditorViewModelDependencies.DomainObjectInteractionService;

      public StringKey DomainObjectEditorKey => _domainObjectEditorKey;

      #endregion

      #region Events

      public event EventHandler<int> DomainObjectSavedEvent;
      public event EventHandler<int> DomainObjectDeletedEvent;
      public event EventHandler DomainObjectReferencesModified;

      #endregion

      #region Commands

      public AsyncRelayCommand DeleteAsyncCommand { get; private set; }
      public AsyncRelayCommand SaveChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelChangesAsyncCommand { get; private set; }
      //public RelayCommand ShowDomainObjectReferenceSelectorsCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectEditorViewModel(DomainObjectViewModel domainObjectViewModel,
                                         DomainObjectEditorViewModelDependencies domainObjectEditorViewModelDependencies)

      {
         _domainObjectViewModel = domainObjectViewModel;
         _domainObjectEditorViewModelDependencies = domainObjectEditorViewModelDependencies;
         _domainObjectService = _domainObjectEditorViewModelDependencies.DomainObjectServiceFactory.CreateDomainObjectService<T>();

         //DomainObjectReferenceSelectorViewModels = new ObservableCollection<DomainObjectReferenceSelectorViewModel>(DomainObjectInteractionService.GetDomainObjectReferenceSelectorViewModels(DomainObjectViewModel));

         /*foreach (var domainObjectReferenceSelectorViewModel in DomainObjectReferenceSelectorViewModels)
         {
            domainObjectReferenceSelectorViewModel.ReferenceSelected += DomainObjectReferenceSelectorViewModel_ReferenceSelected;
         }*/

         DomainObjectReferencesEditorViewModel = new DomainObjectReferencesEditorViewModel<T>(DomainObjectViewModel, domainObjectEditorViewModelDependencies);

         DeleteAsyncCommand = new AsyncRelayCommand(DeleteAsync, CanDelete);
         SaveChangesAsyncCommand = new AsyncRelayCommand(SaveChangesAsync, CanSaveChanges);
         CancelChangesAsyncCommand = new AsyncRelayCommand(CancelChangesAsync, CanCancelChanges);
         //ShowDomainObjectReferenceSelectorsCommand = new RelayCommand(ShowDomainObjectReferenceSelectors);
         

         _domainObjectViewModel.PropertyChanged += DomainObjectViewModel_PropertyChanged;

         IsEnabled = true;

         _nestedDomainObjectTreeViewModels = new ObservableCollection<DomainObjectTreeViewModel>();
         //_domainObjectReferenceViewModels = new ObservableCollection<DomainObjectReferenceViewModel>();
         _processWorkflowViewModels = new ObservableCollection<ProcessWorkflowViewModel>();
      }

      #endregion

      #region Public Methods

      public virtual async Task InitializeAsync()
      {
         await Task.CompletedTask;
      }

      public virtual bool CanSaveChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual async Task SaveChangesAsync()
      {
         IsEnabled = false;

         if (HasUnsavedChanges)
         {
            _domainObjectViewModel.SetPropertiesToDomainObject();

            if (await DomainObjectServiceHelper.TrySaveAsync((T)_domainObjectViewModel.DomainObject, _domainObjectService, _domainObjectEditorViewModelDependencies.UserDialogService, _domainObjectEditorViewModelDependencies.LocalizationService))
            {
               _domainObjectViewModel.RefreshTrackingInfos();

               foreach (var nestedTreeViewModel in _nestedDomainObjectTreeViewModels)
               {
                  await nestedTreeViewModel.SaveChangesAsync();
               }
            }

            _domainObjectViewModel.ResetState();
         }

         HasUnsavedChanges = false;

         IsEnabled = true;

         ExecutePostChangeActions();
      }

      public virtual bool CanCancelChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual async Task CancelChangesAsync()
      {
         IsEnabled = false;

         _domainObjectViewModel.InitializeProperties();

         foreach (var nestedTreeViewModel in _nestedDomainObjectTreeViewModels)
         {
            await nestedTreeViewModel.CancelChangesAsync();
         }

         _domainObjectViewModel.ResetState();

         HasUnsavedChanges = false;

         IsEnabled = true;

         ExecutePostChangeActions();
      }

      public virtual async Task LoadNestedStructuresAsync()
      {
         RefreshWorkflows();

         /*foreach (var domainObjectReferenceSelectorViewModel in DomainObjectReferenceSelectorViewModels)
         {
            await domainObjectReferenceSelectorViewModel.DomainObjectSelectorViewModel.InitializeAsync();
         }*/
      }

      public async Task LoadDomainObjetReferencesAsync()
      {
         await DomainObjectReferencesEditorViewModel.InitializeAsync();

         /*await _loadDomainObjectReferencesLock.WaitAsync();

         try
         {
            DomainObjectReferenceViewModels.Clear();

            var domainObjectReferences = await _domainObjectService.GetDomainObjectReferencesAsync(_domainObjectViewModel.DomainObject.Id);

            foreach (var domainObjectReference in domainObjectReferences)
            {
               var domainObjectReferenceViewModel = new DomainObjectReferenceViewModel(domainObjectReference, DomainObjectInteractionService);
               DomainObjectReferenceViewModels.Add(domainObjectReferenceViewModel);
            }
         }
         finally
         {
            _loadDomainObjectReferencesLock.Release();
         }*/
      }

      public virtual async Task<DomainObject?> DupplicateAsync(DomainObject? parent)
      {
         var dupplicatedDomainObject = (T)DomainObjectViewModel.DomainObject.Clone();

         DomainObjectViewModel.CopyPropertiesToDomainObject(dupplicatedDomainObject);

         var domainObjectService = _domainObjectEditorViewModelDependencies.DomainObjectServiceFactory.CreateDomainObjectService<T>();

         if (await DomainObjectServiceHelper.TryCreateAsync(dupplicatedDomainObject, parent, domainObjectService, _domainObjectEditorViewModelDependencies.UserDialogService, _domainObjectEditorViewModelDependencies.LocalizationService))
         {
            if (dupplicatedDomainObject is IIndexable indexableDupplicatedDomainObject)
            {
               indexableDupplicatedDomainObject.Index = ((IIndexable)DomainObjectViewModel.DomainObject).Index;
               await DomainObjectServiceHelper.TrySaveAsync(dupplicatedDomainObject, domainObjectService, _domainObjectEditorViewModelDependencies.UserDialogService, _domainObjectEditorViewModelDependencies.LocalizationService);
            }

            return dupplicatedDomainObject;
         }
         else
         {
            return null;
         }
      }

      public virtual void Dispose()
      {
         UnRegisterNestedStructures();

         /*DomainObjectReferenceViewModels.ToList().ForEach(vm => vm.Dispose());
         DomainObjectReferenceViewModels.Clear();

         _domainObjectViewModel.PropertyChanged -= DomainObjectViewModel_PropertyChanged;

         /*foreach (var domainObjectReferenceSelectorViewModel in DomainObjectReferenceSelectorViewModels)
         {
            domainObjectReferenceSelectorViewModel.ReferenceSelected -= DomainObjectReferenceSelectorViewModel_ReferenceSelected;
         }

         DomainObjectReferenceSelectorViewModels.ToList().ForEach(vm => vm.Dispose());*/
      }

      #endregion

      #region Private Methods

      protected void ExecutePostChangeActions()
      {
         NotifyCommandsCanExecuteChanged();
         RefreshWorkflows();
         RaiseDomainObjectSavedEvent();
      }

      protected void RaiseDomainObjectSavedEvent()
      {
         DomainObjectSavedEvent?.Invoke(this, _domainObjectViewModel.DomainObject.Id);
      }

      /*private async void DomainObjectReferenceSelectorViewModel_ReferenceSelected(object? sender, ReferenceSelectedEventArgs e)
      {
         var domainObjectReference = await _domainObjectService.CreateDomainObjectReferenceAsync(DomainObjectViewModel.DomainObject.Id, e.DomainObjectId, e.ReferenceType);

         await LoadDomainObjetReferencesAsync();

         DomainObjectReferencesModified?.Invoke(this, EventArgs.Empty);
      }*/

      /*private void ShowDomainObjectReferenceSelectors()
      {
         _domainObjectEditorViewModelDependencies.WindowService.ShowDomainObjectReferenceSelectors(DomainObjectReferenceSelectorViewModels.ToList(), DisplayType.Modal);
      }*/      

      protected void RefreshWorkflows()
      {
         foreach (var workflowViewModel in _processWorkflowViewModels)
         {
            workflowViewModel.RefreshWorkflowState();
         }
      }

      protected void RegisterNestedStructures()
      {
         UnRegisterNestedStructures();

         foreach (var nestedDomainObjectTreeViewModel in _nestedDomainObjectTreeViewModels)
         {
            nestedDomainObjectTreeViewModel.PropertyChanged += NestedDomainObjectTreeViewModel_PropertyChanged;
            nestedDomainObjectTreeViewModel.TreeModified += NestedDomainObjectTreeViewModel_TreeModified;
         }
      }

      private void UnRegisterNestedStructures()
      {
         foreach (var nestedDomainObjectTreeViewModel in _nestedDomainObjectTreeViewModels)
         {
            nestedDomainObjectTreeViewModel.PropertyChanged -= NestedDomainObjectTreeViewModel_PropertyChanged;
            nestedDomainObjectTreeViewModel.TreeModified -= NestedDomainObjectTreeViewModel_TreeModified;
         }
      }

      protected virtual bool CanDelete()
      {
         return DomainObjectViewModel != null && !HasUnsavedChanges;
      }

      protected virtual async Task DeleteAsync()
      {
         var confirmation = _domainObjectEditorViewModelDependencies.UserDialogService.Confirm("Confirmez-vous la suppression ?");

         if (confirmation)
         {
            var domainObjectId = DomainObjectViewModel.DomainObject.Id;

            if (await DomainObjectServiceHelper.TryDeleteAsync((T)DomainObjectViewModel.DomainObject, _domainObjectService, _domainObjectEditorViewModelDependencies.UserDialogService, _domainObjectEditorViewModelDependencies.LocalizationService))
            {
               DomainObjectDeletedEvent?.Invoke(this, domainObjectId);
               RefreshWorkflows();
            }
         }
      }

      protected void DomainObjectViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         var property = sender.GetType().GetProperty(e.PropertyName);

         if (property != null && property.GetCustomAttribute<MonitoredProperty>() != null && !((DomainObjectViewModel)sender).IsInitializingProperties)
         {
            HasUnsavedChanges = true;
            RefreshWorkflows();
         }

         NotifyCommandsCanExecuteChanged();
      }

      private void NestedDomainObjectTreeViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(DomainObjectTreeViewModel.HasUnsavedChanges)) && !HasUnsavedChanges)
         {
            HasUnsavedChanges = _nestedDomainObjectTreeViewModels.Any(vm => vm.HasUnsavedChanges);
         }

         NotifyCommandsCanExecuteChanged();
      }

      protected virtual void NestedDomainObjectTreeViewModel_TreeModified(object? sender, EventArgs e)
      {
         RefreshWorkflows();
      }

      protected virtual void NotifyCommandsCanExecuteChanged()
      {
         DeleteAsyncCommand.NotifyCanExecuteChanged();
         SaveChangesAsyncCommand.NotifyCanExecuteChanged();
         CancelChangesAsyncCommand.NotifyCanExecuteChanged();
      }
      #endregion
   }
}
