using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.Services;
using OtherSideCore.Domain;

namespace OtherSideCore.Adapter.Views
{
    public class SingleDomainObjectBrowserWorkspace : Workspace
   {
      #region Fields

      private IDomainObjectBrowserViewModel _browserViewModel;
      private StringKey _domainObjectBrowserKey;
      protected IDomainObjectInteractionService _domainObjectInteractionService;

      #endregion

      #region Properties

      public IDomainObjectBrowserViewModel IDomainObjectBrowserViewModel => BrowserViewModel;

      public IDomainObjectBrowserViewModel BrowserViewModel
      {
         get => _browserViewModel;
         set => SetProperty(ref _browserViewModel, value);
      }

      public override bool HasUnsavedChanges => BrowserViewModel.HasUnsavedChanges;

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public SingleDomainObjectBrowserWorkspace(
         StringKey domainObjectBrowserKey,
         IDomainObjectInteractionService domainObjectInteractionFactory) :
         base()
      {
         _domainObjectInteractionService = domainObjectInteractionFactory;
         _domainObjectBrowserKey = domainObjectBrowserKey;

         CreateBrowserViewModel();

         BrowserViewModel.PropertyChanged += BrowserViewModel_PropertyChanged;
      }

      #endregion

      #region Public Methods

      public override async Task InitializeAsync(int? domainObjectId = null)
      {
         await BrowserViewModel.InitializeAsync(domainObjectId);
      }

      public override bool CanCancelChanges()
      {
         return BrowserViewModel.CanCancelChanges();
      }

      public override async Task CancelChangesAsync()
      {
         await BrowserViewModel.CancelChangesAsync();
      }

      public override bool CanSaveChanges()
      {
         return BrowserViewModel.CanSaveChanges();
      }

      public override async Task SaveChangesAsync()
      {
         await BrowserViewModel.SaveChangesAsync();
      }

      public override void Dispose()
      {
         BrowserViewModel.PropertyChanged -= BrowserViewModel_PropertyChanged;
         BrowserViewModel.Dispose();
      }

      #endregion

      #region Private Methods

      protected virtual void CreateBrowserViewModel()
      {
         BrowserViewModel = _domainObjectInteractionService.CreateDomainObjectBrowserViewModel(_domainObjectBrowserKey);
      }

      private void BrowserViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(BrowserViewModel.HasUnsavedChanges)))
         {
            OnPropertyChanged(nameof(HasUnsavedChanges));
         }
      }

      #endregion
   }
}
