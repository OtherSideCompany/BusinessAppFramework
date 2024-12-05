using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
    public class DomainObjectEditorViewModel<T> : UIInteractionHost, IDomainObjectEditorViewModel where T : DomainObject, new()
   {
      #region Fields

      protected DomainObjectViewModel _domainObjectViewModel;
      protected IDomainObjectService<T> _domainObjectService;
      protected IDomainObjectServiceFactory _domainObjectServiceFactory;
      protected IDomainObjectInteractionFactory _domainObjectInteractionFactory;

      protected ObservableCollection<DomainObjectTreeViewModel> _nestedDomainObjectTreeViewModels;

      private bool _isEnabled;
      private bool _hasUnsavedChanges;

      #endregion

      #region Properties


      public DomainObjectViewModel DomainObjectViewModel => _domainObjectViewModel;

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

      public event EventHandler<DomainObjectViewModel> DomainObjectDeletedEvent;

      #endregion

      #region Commands

      public AsyncRelayCommand<DomainObjectViewModel> DeleteAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectEditorViewModel(DomainObjectViewModel domainObjectViewModel,
                                         IDomainObjectServiceFactory domainObjectServiceFactory,
                                         IDomainObjectInteractionFactory domainObjectInteractionFactory,
                                         IUserDialogService userDialogService,
                                         IWindowService windowService) :
         base (userDialogService, windowService)
      {
         _domainObjectViewModel = domainObjectViewModel;
         _domainObjectServiceFactory = domainObjectServiceFactory;
         _domainObjectService = _domainObjectServiceFactory.CreateDomainObjectService<T>();
         _domainObjectInteractionFactory = domainObjectInteractionFactory;
         _userDialogService = userDialogService;
         _windowService = windowService;

         DeleteAsyncCommand = new AsyncRelayCommand<DomainObjectViewModel>(DeleteAsync, CanDelete);

         _domainObjectViewModel.PropertyChanged += DomainObjectViewModel_PropertyChanged;

         IsEnabled = true;

         _nestedDomainObjectTreeViewModels = new ObservableCollection<DomainObjectTreeViewModel>();
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
      }

      public virtual async Task LoadNestedStructuresAsync()
      {

      }

      public virtual void Dispose()
      {
         UnRegisterNestedStructures();
         _domainObjectViewModel.PropertyChanged -= DomainObjectViewModel_PropertyChanged;
      }

      #endregion

      #region Private Methods

      protected void RegisterNestedStructures()
      {
         UnRegisterNestedStructures();

         foreach (var nestedDomainObjectTreeViewModel in _nestedDomainObjectTreeViewModels)
         {
            nestedDomainObjectTreeViewModel.PropertyChanged += NestedDomainObjectTreeViewModel_PropertyChanged;
         }
      }

      private void UnRegisterNestedStructures()
      {
         foreach (var nestedDomainObjectTreeViewModel in _nestedDomainObjectTreeViewModels)
         {
            nestedDomainObjectTreeViewModel.PropertyChanged -= NestedDomainObjectTreeViewModel_PropertyChanged;
         }
      }

      protected virtual bool CanDelete(DomainObjectViewModel domainObjectViewModel)
      {
         return domainObjectViewModel != null;
      }

      private async Task DeleteAsync(DomainObjectViewModel domainObjectViewModel)
      {
         var confirmation = _userDialogService.Confirm("Confirmez-vous la suppression ?");

         if (confirmation)
         {
            await _domainObjectService.DeleteAsync((T)domainObjectViewModel.DomainObject);
            DomainObjectDeletedEvent?.Invoke(this, domainObjectViewModel);
         }
      }

      protected void DomainObjectViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         var property = sender.GetType().GetProperty(e.PropertyName);

         if (property != null && property.GetCustomAttribute<MonitoredPropertyAttribute>() != null)
         {
            HasUnsavedChanges = true;
         }
      }

      private void NestedDomainObjectTreeViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(DomainObjectTreeViewModel.HasUnsavedChanges)) && !HasUnsavedChanges)
         {
            HasUnsavedChanges = _nestedDomainObjectTreeViewModels.Any(vm => vm.HasUnsavedChanges);
         }
      }

      protected virtual void NotifyCommandsCanExecuteChanged()
      {
         DeleteAsyncCommand.NotifyCanExecuteChanged();
      }
      #endregion
   }
}
