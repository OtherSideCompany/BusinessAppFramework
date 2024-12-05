using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application.DomainObjectBrowser;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public class DomainObjectSelectorViewModel<T> : DomainObjectBrowserViewModel<T>, IDomainObjectSelectorViewModel where T : DomainObject, new()
   {
      #region Fields



      #endregion

      #region Properties

      public bool DynamicSearch { get; set; }

      #endregion

      #region Commands

      public RelayCommand ValidateSelectionCommand { get; private set; }

      #endregion

      #region Events

      public EventHandler SelectionValidated;

      #endregion

      #region Constructor

      public DomainObjectSelectorViewModel(DomainObjectBrowser<T> domainObjectBrowser,
                                           IDomainObjectViewModelFactory domainObjectViewModelFactory,
                                           IUserDialogService userDialogService,
                                           IDomainObjectsSearchViewModelFactory domainObjectsSearchViewModelFactory,
                                           IWindowService windowService,
                                           IDomainObjectInteractionFactory domainObjectInteractionFactory) :
         base(domainObjectBrowser,
              domainObjectViewModelFactory,
              userDialogService,
              domainObjectsSearchViewModelFactory,
              windowService,
              domainObjectInteractionFactory)
      {
         DomainObjectsSearchViewModel.SingleTextFilterViewModel.PropertyChanged += SingleTextFilterViewModel_PropertyChanged;

         ValidateSelectionCommand = new RelayCommand(ValidateSelection, CanValidateSelection);

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
      }

      public override void Dispose()
      {
         base.Dispose();

         DomainObjectsSearchViewModel.SingleTextFilterViewModel.PropertyChanged -= SingleTextFilterViewModel_PropertyChanged;

         Selection.PropertyChanged -= Selection_PropertyChanged;
      }


      #endregion

      #region Private Methods

      private async void SingleTextFilterViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (DynamicSearch && e.PropertyName.Equals(nameof(SingleTextFilterViewModel.Filter)))
         {
            await DomainObjectsSearchViewModel.PaginatedSearchAsync(new PaginatedSearchParameters() { ResetPage = true });
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
