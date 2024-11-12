using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.DomainObjectBrowser;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;
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
      protected IWindowService _windowService;

      protected ObservableCollection<IDomainObjectBrowserViewModel> _nestedDomainObjectBrowserViewModels;

      private bool _isEnabled;
      private bool _hasUnsavedChanges;

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
                                         IUserDialogService userDialogService,
                                         IWindowService windowService)
      {
         _domainObjectViewModel = domainObjectViewModel;
         _domainObjectService = domainObjectService;
         _userDialogService = userDialogService;
         _windowService = windowService;

         DeleteAsyncCommand = new AsyncRelayCommand<DomainObjectViewModel>(DeleteAsync, CanDelete);

         _domainObjectViewModel.PropertyChanged += DomainObjectViewModel_PropertyChanged;

         IsEnabled = true;

         _nestedDomainObjectBrowserViewModels = new ObservableCollection<IDomainObjectBrowserViewModel>();
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

            foreach (var nestedBrowserViewModel in NestedDomainObjectBrowserViewModels)
            {
               await nestedBrowserViewModel.SaveChangesAsync();
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

         foreach (var nestedBrowserViewModel in NestedDomainObjectBrowserViewModels)
         {
            await nestedBrowserViewModel.CancelChangesAsync();
         }

         _domainObjectViewModel.ResetState();

         HasUnsavedChanges = false;

         IsEnabled = true;
      }

      public virtual async Task LoadNestedBrowsersAsync()
      {

      }

      public virtual void Dispose()
      {
         UnRegisterNestedDomainObjectBrowserViewModel();
         _domainObjectViewModel.PropertyChanged -= DomainObjectViewModel_PropertyChanged;
      }

      #endregion

      #region Private Methods

      protected void RegisterNestedDomainObjectBrowserViewModel()
      {
         UnRegisterNestedDomainObjectBrowserViewModel();

         foreach (var nestedDomainObjectBrowserViewModel in _nestedDomainObjectBrowserViewModels)
         {
            nestedDomainObjectBrowserViewModel.PropertyChanged += NestedDomainObjectBrowserViewModel_PropertyChanged;
         }
      }

      private void UnRegisterNestedDomainObjectBrowserViewModel()
      {
         foreach (var nestedDomainObjectBrowserViewModel in _nestedDomainObjectBrowserViewModels)
         {
            nestedDomainObjectBrowserViewModel.PropertyChanged -= NestedDomainObjectBrowserViewModel_PropertyChanged;
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

      private void NestedDomainObjectBrowserViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(DomainObjectBrowserViewModel<T>.HasUnsavedChanges)) && !HasUnsavedChanges)
         {
            HasUnsavedChanges = _nestedDomainObjectBrowserViewModels.Any(vm => vm.HasUnsavedChanges);
         }
      }
      #endregion
   }
}
