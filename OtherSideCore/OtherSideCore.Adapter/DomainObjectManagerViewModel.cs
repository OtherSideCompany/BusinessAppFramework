using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter
{
   public class DomainObjectManagerViewModel<T> : ObservableObject, IDisposable where T : DomainObject, new()
   {
      #region Fields

      private IDomainObjectService<T> _domainObjectService;
      private IUserContext _userContext;
      private IUserDialogService _userDialogService;
      private IDomainObjectViewModelFactory _viewModelFactory;
      private bool _isEnabled;

      private DomainObjectViewModel _selectedDomainObjectViewModel;

      #endregion

      #region Properties

      public DomainObjectViewModel SelectedDomainObjectViewModel
      {
         get => _selectedDomainObjectViewModel;
         set
         { 
            SetProperty(ref _selectedDomainObjectViewModel, value);
            NotifyCommandCanExecuteChange();
         }
      }

      public bool IsEnabled
      {
         get => _isEnabled;
         set => SetProperty(ref _isEnabled, value);
      }

      #endregion

      #region Events

      public event EventHandler<DomainObjectViewModel> DomainObjectCreated;
      public event EventHandler<DomainObjectViewModel> DomainObjectDeleted;

      #endregion

      #region Commands

      public AsyncRelayCommand CreateAsyncCommand { get; private set; }

      public AsyncRelayCommand DeleteAsyncCommand { get; private set; }

      public AsyncRelayCommand<DomainObjectViewModel> DeleteDomainObjectAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectManagerViewModel(IUserContext userContext, IDomainObjectService<T> domainObjectService, IUserDialogService userDialogService, IDomainObjectViewModelFactory viewModelFactory)
      {
         _userContext = userContext;
         _domainObjectService = domainObjectService;
         _userDialogService = userDialogService;
         _viewModelFactory = viewModelFactory;

         CreateAsyncCommand = new AsyncRelayCommand(CreateAsync);
         DeleteAsyncCommand = new AsyncRelayCommand(DeleteAsync, CanDelete);
         DeleteDomainObjectAsyncCommand = new AsyncRelayCommand<DomainObjectViewModel>(DeleteDomainObjectAsync);

         IsEnabled = true;
      }

      #endregion

      #region Public Methods

      public virtual void Dispose()
      {

      }

      #endregion

      #region Private Methods

      private async Task CreateAsync()
      {
         var domainObject = new T();
         await _domainObjectService.CreateAsync(domainObject);
         await _domainObjectService.LoadAsync(domainObject);

         var viewModel = _viewModelFactory.CreateViewModel(domainObject);

         DomainObjectCreated?.Invoke(this, viewModel);
      }

      protected virtual bool CanDelete()
      {
         return SelectedDomainObjectViewModel != null;
      }

      private async Task DeleteAsync()
      {
         var confirmation = _userDialogService.Confirm("Confirmez-vous la suppression ?");

         if (confirmation)
         {
            await _domainObjectService.DeleteAsync((T)SelectedDomainObjectViewModel.DomainObject);            

            DomainObjectDeleted?.Invoke(this, SelectedDomainObjectViewModel);

            SelectedDomainObjectViewModel = null;
         }
      }

      private async Task DeleteDomainObjectAsync(DomainObjectViewModel domainObjectViewModel)
      {
         var confirmation = _userDialogService.Confirm("Confirmez-vous la suppression ?");

         if (confirmation)
         {
            await _domainObjectService.DeleteAsync((T)domainObjectViewModel.DomainObject);

            DomainObjectDeleted?.Invoke(this, domainObjectViewModel);
         }
      }

      private void NotifyCommandCanExecuteChange()
      {
         CreateAsyncCommand.NotifyCanExecuteChanged();
         DeleteAsyncCommand.NotifyCanExecuteChanged();
      }

      #endregion
   }
}
