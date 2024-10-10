using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using System.ComponentModel;
using System.Reflection;

namespace OtherSideCore.Adapter
{
   public class DomainObjectEditorViewModel<T> : ObservableObject, IDisposable where T : DomainObject, new()
   {
      #region Fields

      protected IDomainObjectService<T> _domainObjectService;
      protected IUserContext _userContext;
      protected DomainObjectViewModel _domainObjectViewModel;
      protected IUserDialogService _userDialogService;
      private bool _hasUnsavedChanges;
      private bool _isEnabled;

      #endregion

      #region Properties

      public DomainObjectViewModel DomainObjectViewModel
      {
         get => _domainObjectViewModel;
         set => SetProperty(ref _domainObjectViewModel, value);
      }

      public bool HasUnsavedChanges
      {
         get => _hasUnsavedChanges;
         set
         {
            SetProperty(ref _hasUnsavedChanges, value);
            NotifyCommandCanExecuteChange();
         }
      }

      public bool IsEnabled
      {
         get => _isEnabled;
         set => SetProperty(ref _isEnabled, value);
      }

      #endregion

      #region Commands
      public AsyncRelayCommand SaveChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelChangesAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectEditorViewModel(IUserContext userContext, IDomainObjectService<T> domainObjectService, DomainObjectViewModel domainObjectViewModel, IUserDialogService userDialogService)
      {
         _userContext = userContext;
         _domainObjectService = domainObjectService;
         _userDialogService = userDialogService;

         DomainObjectViewModel = domainObjectViewModel;

         SaveChangesAsyncCommand = new AsyncRelayCommand(SaveChangesAsync, CanSaveChanges);
         CancelChangesAsyncCommand = new AsyncRelayCommand(CancelChangesAsync, CanCancelChanges);

         DomainObjectViewModel.PropertyChanged += DomainObjectViewModel_PropertyChanged;

         IsEnabled = true;
      }

      #endregion

      #region Public Methods

      public void Dispose()
      {
         DomainObjectViewModel.PropertyChanged -= DomainObjectViewModel_PropertyChanged;
      }

      #endregion

      #region Private Methods

      private void DomainObjectViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         var property = DomainObjectViewModel.GetType().GetProperty(e.PropertyName);

         if (property != null && property.GetCustomAttribute<MonitoredPropertyAttribute>() != null)
         {
            HasUnsavedChanges = true;
         }
      }

      protected virtual bool CanSaveChanges()
      {
         return HasUnsavedChanges;
      }

      protected virtual async Task SaveChangesAsync()
      {
         IsEnabled = false;

         DomainObjectViewModel.SetPropertiesToDomainObject();
         await _domainObjectService.SaveAsync((T)DomainObjectViewModel.DomainObject);
         await _domainObjectService.LoadTrackingInfosAsync((T)DomainObjectViewModel.DomainObject);
         DomainObjectViewModel.RefreshTrackingInfos();
         HasUnsavedChanges = false;

         IsEnabled = true;
      }

      protected virtual bool CanCancelChanges()
      {
         return HasUnsavedChanges;
      }

      protected virtual async Task CancelChangesAsync()
      {
         IsEnabled = false;

         DomainObjectViewModel.InitializeProperties();
         HasUnsavedChanges = false;

         IsEnabled = true;
      }

      private void NotifyCommandCanExecuteChange()
      {
         SaveChangesAsyncCommand.NotifyCanExecuteChanged();

         CancelChangesAsyncCommand.NotifyCanExecuteChanged();
      }
      #endregion
   }
}
