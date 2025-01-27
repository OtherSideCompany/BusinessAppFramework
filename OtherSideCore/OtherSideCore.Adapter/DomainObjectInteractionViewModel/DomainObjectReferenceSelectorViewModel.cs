using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Adapter.DomainObjectInteraction;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public class DomainObjectReferenceSelectorViewModel : ObservableObject, IDisposable
   {
      #region Fields

      private Type _referenceType;
      private string _referenceTypeName;
      private IDomainObjectSelectorViewModel _domainObjectSelectorViewModel;

      #endregion

      #region Properties

      public Type ReferenceType
      {
         get => _referenceType;
         private set => SetProperty(ref _referenceType, value);
      }

      public string ReferenceTypeName
      {
         get => _referenceTypeName;
         private set => SetProperty(ref _referenceTypeName, value);
      }

      public IDomainObjectSelectorViewModel DomainObjectSelectorViewModel
      {
         get => _domainObjectSelectorViewModel;
         private set => SetProperty(ref _domainObjectSelectorViewModel, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Events

      public EventHandler<ReferenceSelectedEventArgs> ReferenceSelected;

      #endregion

      #region Constructor

      public DomainObjectReferenceSelectorViewModel(Type referenceType, string referenceTypeName, IDomainObjectSelectorViewModel domainObjectSelectorViewModel)
      {
         ReferenceType = referenceType;
         ReferenceTypeName = referenceTypeName;
         DomainObjectSelectorViewModel = domainObjectSelectorViewModel;

         DomainObjectSelectorViewModel.SelectionValidated += DomainObjectSelectorViewModel_SelectionValidated;
      }      

      #endregion

      #region Public Methods

      public void Dispose()
      {
         DomainObjectSelectorViewModel.Dispose();
      }

      #endregion

      #region Private Methods

      private void DomainObjectSelectorViewModel_SelectionValidated(object? sender, EventArgs e)
      {
         if (!DomainObjectSelectorViewModel.Selection.IsSelectionEmpty)
         {
            ReferenceSelected?.Invoke(this, new ReferenceSelectedEventArgs(((DomainObjectSearchResultViewModel)DomainObjectSelectorViewModel.Selection.SelectedItem).DomainObjectSearchResult.DomainObjectId, ReferenceType));
         }
      }

      #endregion
   }
}
