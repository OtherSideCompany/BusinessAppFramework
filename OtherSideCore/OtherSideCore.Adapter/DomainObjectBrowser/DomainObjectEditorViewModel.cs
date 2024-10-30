using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.DomainObjectBrowser;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using System.ComponentModel;
using System.Reflection;

namespace OtherSideCore.Adapter.DomainObjectBrowser
{
   public class DomainObjectEditorViewModel<T> : ObservableObject, IDomainObjectEditorViewModel where T : DomainObject, new()
   {
      #region Fields

      protected DomainObjectViewModel _domainObjectViewModel;
      protected IDomainObjectService<T> _domainObjectService;
      protected IUserDialogService _userDialogService;

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
                                         IDomainObjectService<T> domainObjectService,
                                         IUserDialogService userDialogService)
      {
         _domainObjectViewModel = domainObjectViewModel;
         _domainObjectService = domainObjectService;
         _userDialogService = userDialogService;

         DeleteAsyncCommand = new AsyncRelayCommand<DomainObjectViewModel>(DeleteAsync, CanDelete);

         _domainObjectViewModel.PropertyChanged += DomainObjectViewModel_PropertyChanged;

         IsEnabled = true;
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
            await _domainObjectService.LoadTrackingInfosAsync((T)_domainObjectViewModel.DomainObject);

            _domainObjectViewModel.RefreshTrackingInfos();
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

         HasUnsavedChanges = false;

         IsEnabled = true;
      }

      public virtual void Dispose()
      {
         _domainObjectViewModel.PropertyChanged -= DomainObjectViewModel_PropertyChanged;
      }

      #endregion

      #region Private Methods

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
      #endregion
   }
}
