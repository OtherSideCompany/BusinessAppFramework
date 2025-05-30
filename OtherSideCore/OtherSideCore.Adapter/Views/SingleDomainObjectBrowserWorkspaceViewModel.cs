using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.ViewDescriptions;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter.Views
{
   public class SingleDomainObjectBrowserWorkspaceViewModel<TDomainObject, TSearchResult> : WorkspaceViewModel 
      where TDomainObject : DomainObject, new()
      where TSearchResult : DomainObjectSearchResult, new()
   {
      #region Fields

      private DomainObjectBrowserViewModel<TDomainObject, TSearchResult> _browserViewModel;
      protected IDomainObjectInteractionService _domainObjectInteractionService;

      #endregion

      #region Properties

      public IDomainObjectBrowserViewModel IDomainObjectBrowserViewModel => BrowserViewModel;

      public DomainObjectBrowserViewModel<TDomainObject, TSearchResult> BrowserViewModel
      {
         get => _browserViewModel;
         set => SetProperty(ref _browserViewModel, value);
      }

      public override bool HasUnsavedChanges => BrowserViewModel.HasUnsavedChanges;

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public SingleDomainObjectBrowserWorkspaceViewModel(IWindowService windowService, IDomainObjectInteractionService domainObjectInteractionFactory) : base()
      {
         _domainObjectInteractionService = domainObjectInteractionFactory;

         WorkspaceDescription = (WorkspaceDescription)windowService.GetDescription(this);

         CreateBrowserViewModel();

         BrowserViewModel.PropertyChanged += BrowserViewModel_PropertyChanged;
      }

      #endregion

      #region Public Methods

      public override async Task InitializeAsync()
      {
         await BrowserViewModel.InitializeAsync();
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
         BrowserViewModel = (DomainObjectBrowserViewModel<TDomainObject, TSearchResult>)_domainObjectInteractionService.CreateDomainObjectBrowserViewModel<TDomainObject>();
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
