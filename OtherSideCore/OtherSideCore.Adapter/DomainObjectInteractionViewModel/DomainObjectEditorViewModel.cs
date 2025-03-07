using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.Workflows;
using OtherSideCore.Application;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Linq;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
    public class DomainObjectEditorViewModel<T> : ObservableObject, IDomainObjectEditorViewModel, ISavable where T : DomainObject, new()
   {
      #region Fields

      protected DomainObjectViewModel _domainObjectViewModel;
      protected IDomainObjectService<T> _domainObjectService;
      protected IDomainObjectServiceFactory _domainObjectServiceFactory;
      protected IDomainObjectInteractionService _domainObjectInteractionService;
      protected IUserDialogService _userDialogService;
      protected IWindowService _windowService;
      private ObservableCollection<DomainObjectReferenceSelectorViewModel> _domainObjectReferenceSelectorViewModels;

      protected ObservableCollection<DomainObjectTreeViewModel> _nestedDomainObjectTreeViewModels;
      protected ObservableCollection<DomainObjectReferenceViewModel> _domainObjectReferenceViewModels;
      protected ObservableCollection<ProcessWorkflowViewModel> _processWorkflowViewModels;

      private bool _isEnabled;
      private bool _hasUnsavedChanges;

      #endregion

      #region Properties


      public DomainObjectViewModel DomainObjectViewModel => _domainObjectViewModel;

      public ObservableCollection<DomainObjectReferenceViewModel> DomainObjectReferenceViewModels => _domainObjectReferenceViewModels;

      public ObservableCollection<DomainObjectTreeViewModel> NestedDomainObjectTreeViewModels => _nestedDomainObjectTreeViewModels;

      public ObservableCollection<DomainObjectReferenceSelectorViewModel> DomainObjectReferenceSelectorViewModels
      {
         get => _domainObjectReferenceSelectorViewModels;
         private set => SetProperty(ref _domainObjectReferenceSelectorViewModels, value);
      }

      public bool IsEnabled
      {
         get => _isEnabled;
         set => SetProperty(ref _isEnabled, value);
      }

      public bool HasUnsavedChanges
      {
         get => _hasUnsavedChanges;
         set => SetProperty(ref _hasUnsavedChanges, value);
      }

      #endregion

      #region Events

      public event EventHandler<int> DomainObjectSavedEvent;
      public event EventHandler<int> DomainObjectDeletedEvent;

      #endregion

      #region Commands

      public AsyncRelayCommand DeleteAsyncCommand { get; private set; }
      public AsyncRelayCommand SaveChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelChangesAsyncCommand { get; private set; }
      public RelayCommand ShowDomainObjectReferenceSelectorsCommand { get; private set; }
      public AsyncRelayCommand<DomainObjectReferenceViewModel> DeleteDomainObjectReferenceAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectEditorViewModel(DomainObjectViewModel domainObjectViewModel,
                                         IDomainObjectServiceFactory domainObjectServiceFactory,
                                         IDomainObjectInteractionService domainObjectInteractionService,
                                         IUserDialogService userDialogService,
                                         IWindowService windowService)

      {
         _domainObjectViewModel = domainObjectViewModel;
         _domainObjectServiceFactory = domainObjectServiceFactory;
         _domainObjectService = _domainObjectServiceFactory.CreateDomainObjectService<T>();
         _domainObjectInteractionService = domainObjectInteractionService;
         _userDialogService = userDialogService;
         _windowService = windowService;

         DomainObjectReferenceSelectorViewModels = new ObservableCollection<DomainObjectReferenceSelectorViewModel>(_domainObjectInteractionService.GetDomainObjectReferenceSelectorViewModels(DomainObjectViewModel));

         foreach (var domainObjectReferenceSelectorViewModel in DomainObjectReferenceSelectorViewModels)
         {
            domainObjectReferenceSelectorViewModel.ReferenceSelected += DomainObjectReferenceSelectorViewModel_ReferenceSelected;
         }

         DeleteAsyncCommand = new AsyncRelayCommand(DeleteAsync, CanDelete);
         SaveChangesAsyncCommand = new AsyncRelayCommand(SaveChangesAsync, CanSaveChanges);
         CancelChangesAsyncCommand = new AsyncRelayCommand(CancelChangesAsync, CanCancelChanges);
         ShowDomainObjectReferenceSelectorsCommand = new RelayCommand(ShowDomainObjectReferenceSelectors);
         DeleteDomainObjectReferenceAsyncCommand = new AsyncRelayCommand<DomainObjectReferenceViewModel>(DeleteDomainObjectReferenceAsync);

         _domainObjectViewModel.PropertyChanged += DomainObjectViewModel_PropertyChanged;

         IsEnabled = true;

         _nestedDomainObjectTreeViewModels = new ObservableCollection<DomainObjectTreeViewModel>();
         _domainObjectReferenceViewModels = new ObservableCollection<DomainObjectReferenceViewModel>();
         _processWorkflowViewModels = new ObservableCollection<ProcessWorkflowViewModel>();
      }      

      #endregion

      #region Public Methods

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

            await _domainObjectService.SaveAsync((T)_domainObjectViewModel.DomainObject);

            _domainObjectViewModel.RefreshTrackingInfos();

            foreach (var nestedTreeViewModel in _nestedDomainObjectTreeViewModels)
            {
               await nestedTreeViewModel.SaveChangesAsync();
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
         DomainObjectReferenceSelectorViewModels.ToList().ForEach(async vm => await vm.DomainObjectSelectorViewModel.InitializeAsync());
      }

      public async Task LoadDomainObjetReferencesAsync()
      {
         DomainObjectReferenceViewModels.Clear();

         var domainObjectReferences = await _domainObjectService.GetDomainObjectReferencesAsync(_domainObjectViewModel.DomainObject.Id);

         foreach (var domainObjectReference in domainObjectReferences)
         {
            var domainObjectReferenceViewModel = new DomainObjectReferenceViewModel(domainObjectReference, _domainObjectInteractionService);
            DomainObjectReferenceViewModels.Add(domainObjectReferenceViewModel);
         }
      }

      public async Task<DomainObject> DupplicateAsync(DomainObject? parent)
      {
         var dupplicatedDomainObject = (T)DomainObjectViewModel.DomainObject.Clone();

         DomainObjectViewModel.CopyPropertiesToDomainObject(dupplicatedDomainObject);

         var domainObjectService = _domainObjectServiceFactory.CreateDomainObjectService<T>();
         await domainObjectService.CreateAsync(dupplicatedDomainObject, parent);

         if (dupplicatedDomainObject is IIndexable indexableDupplicatedDomainObject)
         {
            indexableDupplicatedDomainObject.Index = ((IIndexable)DomainObjectViewModel.DomainObject).Index;
            await domainObjectService.SaveAsync(dupplicatedDomainObject);
         }

         return dupplicatedDomainObject;
      }

      public virtual void Dispose()
      {
         UnRegisterNestedStructures();

         DomainObjectReferenceViewModels.ToList().ForEach(vm => vm.Dispose());
         DomainObjectReferenceViewModels.Clear();

         _domainObjectViewModel.PropertyChanged -= DomainObjectViewModel_PropertyChanged;

         foreach (var domainObjectReferenceSelectorViewModel in DomainObjectReferenceSelectorViewModels)
         {
            domainObjectReferenceSelectorViewModel.ReferenceSelected -= DomainObjectReferenceSelectorViewModel_ReferenceSelected;
         }

         DomainObjectReferenceSelectorViewModels.ToList().ForEach(vm => vm.Dispose());
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

      private async void DomainObjectReferenceSelectorViewModel_ReferenceSelected(object? sender, ReferenceSelectedEventArgs e)
      {
         var domainObjectReference = await _domainObjectService.CreateDomainObjectReferenceAsync(DomainObjectViewModel.DomainObject.Id, e.DomainObjectId, e.ReferenceType);

         await LoadDomainObjetReferencesAsync();
      }

      private void ShowDomainObjectReferenceSelectors()
      {
         _windowService.ShowDomainObjectReferenceSelectors(DomainObjectReferenceSelectorViewModels.ToList(), DisplayType.Modal);
      }

      protected virtual async Task DeleteDomainObjectReferenceAsync(DomainObjectReferenceViewModel? domainObjectReferenceViewModel)
      {
         if (_userDialogService.Confirm("Supprimer la référence ?"))
         {
            await _domainObjectService.DeleteDomainObjectReferenceAsync(DomainObjectViewModel.DomainObject.Id, domainObjectReferenceViewModel.DomainObjectReference);

            DomainObjectReferenceViewModels.Remove(domainObjectReferenceViewModel);
         }
      }

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
         var confirmation = _userDialogService.Confirm("Confirmez-vous la suppression ?");

         if (confirmation)
         {
            var domainObjectId = DomainObjectViewModel.DomainObject.Id;
            await _domainObjectService.DeleteAsync((T)DomainObjectViewModel.DomainObject);
            DomainObjectDeletedEvent?.Invoke(this, domainObjectId);

            RefreshWorkflows();
         }
      }

      protected void DomainObjectViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         var property = sender.GetType().GetProperty(e.PropertyName);

         if (property != null && property.GetCustomAttribute<MonitoredPropertyAttribute>() != null && !((DomainObjectViewModel)sender).IsInitializingProperties)
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

      private void NestedDomainObjectTreeViewModel_TreeModified(object? sender, EventArgs e)
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
