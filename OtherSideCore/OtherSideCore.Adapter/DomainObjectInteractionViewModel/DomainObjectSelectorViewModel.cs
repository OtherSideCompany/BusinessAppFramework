using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Application.Browser;
using OtherSideCore.Application.Factories;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public class DomainObjectSelectorViewModel<T> : DomainObjectBrowserViewModel<T>, IDomainObjectSelectorViewModel where T : DomainObject, new()
   {
      #region Fields

      private IWindowService _windowService;

      #endregion

      #region Properties

      public bool DynamicSearch { get; set; }
      public bool HideOnValidate { get; set; } = true;

      #endregion

      #region Commands

      public RelayCommand ValidateSelectionCommand { get; private set; }
      public AsyncRelayCommand DisplaySelectorAsyncCommand { get; private set; }
      public AsyncRelayCommand<DomainObjectViewModel> DisplaySelectorInContextAsyncCommand { get; private set; }
      public AsyncRelayCommand DisplayDomainObjectBrowserAsyncCommand { get; private set; }
      #endregion

      #region Events

      public event EventHandler SelectionValidated;

      #endregion

      #region Constructor

      public DomainObjectSelectorViewModel(DomainObjectBrowser<T> domainObjectBrowser,
                                           IDomainObjectSearchResultViewModelFactory domainObjectSearchResultViewModelFactory,
                                           IUserDialogService userDialogService,
                                           IDomainObjectsSearchViewModelFactory domainObjectsSearchViewModelFactory,
                                           IWindowService windowService,
                                           IDomainObjectInteractionService domainObjectInteractionFactory,
                                           IDomainObjectServiceFactory domainObjectServiceFactory) :
         base(domainObjectBrowser,
              domainObjectsSearchViewModelFactory,
              domainObjectSearchResultViewModelFactory,
              domainObjectInteractionFactory,
              domainObjectServiceFactory)
      {
         _windowService = windowService;

         _constructEditorOnSelectSearchResult = false;

         ((DomainObjectsSearchViewModel<T>)DomainObjectSearchViewModel).SingleTextFilterViewModel.PropertyChanged += SingleTextFilterViewModel_PropertyChanged;

         ValidateSelectionCommand = new RelayCommand(ValidateSelection, CanValidateSelection);
         DisplaySelectorAsyncCommand = new AsyncRelayCommand(DisplaySelectorAsync);
         DisplaySelectorInContextAsyncCommand = new AsyncRelayCommand<DomainObjectViewModel>(DisplaySelectorInContextAsync);
         DisplayDomainObjectBrowserAsyncCommand = new AsyncRelayCommand(DisplayDomainObjectBrowserAsync);

         Selection.PropertyChanged += Selection_PropertyChanged;

         _loadNestedStructureOnSelection = false;
      }      

      #endregion

      #region Public Methods

      public bool CanValidateSelection()
      {
         return !Selection.IsSelectionEmpty;
      }

      public void ValidateSelection()
      {
         SelectionValidated?.Invoke(this, EventArgs.Empty);

         if (HideOnValidate)
         {
            _windowService.HideTopModal();
         }
      }

      public override void Dispose()
      {
         base.Dispose();

         ((DomainObjectsSearchViewModel<T>)DomainObjectSearchViewModel).SingleTextFilterViewModel.PropertyChanged -= SingleTextFilterViewModel_PropertyChanged;

         Selection.PropertyChanged -= Selection_PropertyChanged;
      }

      public async Task<T> GetSelectedSearchResultDomainObjectAsync()
      {
         var domainObjectSearchResultViewModel = (DomainObjectSearchResultViewModel)Selection.SelectedItem;
         var domainObject = await _domainObjectServiceFactory.CreateDomainObjectService<T>().GetAsync(domainObjectSearchResultViewModel.DomainObjectSearchResult.DomainObjectId);

         return domainObject;
      }


      #endregion

      #region Private Methods

      protected virtual async Task DisplayDomainObjectBrowserAsync()
      {
         if (Selection.IsSelectionEmpty)
         {
            await _domainObjectInteractionService.DisplayDomainObjectBrowserAsync(typeof(T), DisplayType.SubWindow);
         }
         else
         {
            var domainObjectSearchResultViewModel = (DomainObjectSearchResultViewModel)Selection.SelectedItem;
            await _domainObjectInteractionService.DisplayDomainObjectAsync(domainObjectSearchResultViewModel.DomainObjectSearchResult.DomainObjectId, typeof(T), DisplayType.SubWindow);
         }
      }

      private async Task DisplaySelectorAsync()
      {
         await _windowService.ShowDomainObjectSelectorViewAsync(this, DisplayType.Modal);
      }

      private async Task DisplaySelectorInContextAsync(DomainObjectViewModel domainObjectViewModel)
      {
         ContextViewModel = domainObjectViewModel;
         await DisplaySelectorAsync();
      }

      private async void SingleTextFilterViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (DynamicSearch && e.PropertyName.Equals(nameof(SingleTextFilterViewModel.Filter)))
         {
            await DomainObjectSearchViewModel.PaginatedSearchAsync(new PaginatedSearchParameters() { ResetPage = true });
         }
      }

      private void Selection_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         NotifyCommandsCanExecuteChanged();
      }

      protected override void NotifyCommandsCanExecuteChanged()
      {
         base.NotifyCommandsCanExecuteChanged();

         ValidateSelectionCommand.NotifyCanExecuteChanged();
      }

      #endregion
   }
}
