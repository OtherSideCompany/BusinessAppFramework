using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Application.Browser;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public class DomainObjectSelectorViewModel<TDomainObject, TSearchResult> : DomainObjectBrowserViewModel<TDomainObject, TSearchResult>, IDomainObjectSelectorViewModel
      where TDomainObject : DomainObject, new()
      where TSearchResult : DomainObjectSearchResult, new()
   {
      #region Fields

      protected DomainObjectSelectorViewModelDependencies _domainObjectSelectorViewModelDependencies;
      protected StringKey _domainObjectSelectorKey;

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

      public DomainObjectSelectorViewModel(
         StringKey domainObjectSelectorKey,
         DomainObjectBrowser<TDomainObject, TSearchResult> domainObjectBrowser,
         DomainObjectSelectorViewModelDependencies domainObjectSelectorViewModelDependencies) :
         base(domainObjectBrowser,
              domainObjectSelectorViewModelDependencies)
      {
         _domainObjectSelectorKey = domainObjectSelectorKey;

         _constructEditorOnSelectSearchResult = false;

         _domainObjectSelectorViewModelDependencies = domainObjectSelectorViewModelDependencies;

         ((DomainObjectSearchViewModel<TSearchResult>)DomainObjectSearchViewModel).SingleTextFilterViewModel.PropertyChanged += SingleTextFilterViewModel_PropertyChanged;

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
            _domainObjectSelectorViewModelDependencies.WindowService.HideTopModal();
         }
      }

      public override void Dispose()
      {
         base.Dispose();

         ((DomainObjectSearchViewModel<TSearchResult>)DomainObjectSearchViewModel).SingleTextFilterViewModel.PropertyChanged -= SingleTextFilterViewModel_PropertyChanged;

         Selection.PropertyChanged -= Selection_PropertyChanged;
      }


      #endregion

      #region Private Methods

      protected virtual async Task DisplayDomainObjectBrowserAsync()
      {
         var browserWorkspaceKey = _domainObjectSelectorViewModelDependencies.DomainObjectInteractionService.SelectorToWorkspaceKeyMappings[_domainObjectSelectorKey];
         var workspace = _domainObjectSelectorViewModelDependencies.WorkspaceFactory.CreateWorkspace(browserWorkspaceKey);

         if (Selection.IsSelectionEmpty)
         {
            await workspace.InitializeAsync();
         }
         else
         {
            var domainObjectSearchResultViewModel = (DomainObjectSearchResultViewModel)Selection.SelectedItem;
            // display specific item
         }

         var session = _domainObjectSelectorViewModelDependencies.WindowService.DisplayView(browserWorkspaceKey, "", workspace, DisplayType.SubWindow);
         await session.WhenClosed;
         workspace.Dispose();
      }

      private async Task DisplaySelectorAsync()
      {
         await InitializeAsync();
         _domainObjectSelectorViewModelDependencies.WindowService.DisplayView(_domainObjectSelectorKey, "", this, DisplayType.Modal);
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
