using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;

namespace OtherSideCore.Adapter.Views
{
   public class Workspace : ObservableObject, ISavable, IDisposable
   {
      #region Fields

      protected bool _hasUnsavedChanges;
      private string _label;
      private string _iconKey;

      #endregion

      #region Properties

      public virtual bool HasUnsavedChanges
      {
         get => _hasUnsavedChanges;
         set { SetProperty(ref _hasUnsavedChanges, value); NotifyCommandsCanExecuteChanged(); }
      }

      public string Label
      {
         get => _label;
         set => SetProperty(ref _label, value);
      }

      public string IconKey
      {
         get => _iconKey;
         set => SetProperty(ref _iconKey, value);
      }

      public List<NavigationItem> NavigationItems { get; }

      #endregion

      #region Events

      public event EventHandler<NavigationItem?> NavigationItemSelected;

      #endregion

      #region Commands

      public RelayCommand<NavigationItem?> SelectNavigationItemCommand { get; private set; }

      #endregion

      #region Constructor

      public Workspace()
      {
         NavigationItems = new List<NavigationItem>();

         SelectNavigationItemCommand = new RelayCommand<NavigationItem?>(SelectNavigationItem);
      }

      #endregion

      #region Public Methods      

      public virtual bool CanCancelChanges() 
      { 
         return HasUnsavedChanges;
      }

      public virtual Task CancelChangesAsync()
      {
         return Task.CompletedTask;
      }

      public virtual bool CanSaveChanges()
      {
         return HasUnsavedChanges;
      }

      public virtual Task SaveChangesAsync()
      {
         return Task.CompletedTask;
      }

      public virtual Task InitializeAsync()
      {
         return Task.CompletedTask;
      }

      public virtual void Dispose()
      {

      }


      #endregion

      #region Private Methods

      protected virtual void NotifyCommandsCanExecuteChanged()
      {

      }

      private void SelectNavigationItem(NavigationItem? navigationItem)
      {
         NavigationItemSelected?.Invoke(this, navigationItem);
      }

      #endregion

   }
}
