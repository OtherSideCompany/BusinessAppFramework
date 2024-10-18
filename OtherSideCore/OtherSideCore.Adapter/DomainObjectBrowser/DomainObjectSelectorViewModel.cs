using OtherSideCore.Application.DomainObjectBrowser;
using OtherSideCore.Application.Services;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectBrowser
{
   public class DomainObjectSelectorViewModel<T> : DomainObjectBrowserViewModel<T>, IDomainObjectSelectorViewModel where T : DomainObject, new()
   {
      #region Fields

      private DomainObjectSelector<T> _domainObjectSelector => (DomainObjectSelector<T>)_domainObjectBrowser;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Events



      #endregion

      #region Constructor

      public DomainObjectSelectorViewModel(DomainObjectSelector<T> domainObjectSelector,
                                           IDomainObjectViewModelFactory domainObjectViewModelFactory,
                                           IUserDialogService userDialogService,
                                           IDomainObjectsSearchViewModelFactory domainObjectsSearchViewModelFactory) :
         base(domainObjectSelector,
              domainObjectViewModelFactory,
              userDialogService,
              domainObjectsSearchViewModelFactory)
      {
         DomainObjectsSearchViewModel.MultiTextFilterViewModel.PropertyChanged += MultiTextFilterViewModel_PropertyChanged;
      }

      #endregion

      #region Public Methods

      public void RequestSearch()
      {
         DomainObjectsSearchViewModel.MultiTextFilterViewModel.RequestSearch();
      }

      public void SelectDomainObjectViewModel(DomainObjectViewModel domainObjectViewModel)
      {
         Selection.SelectViewModel(domainObjectViewModel);
      }

      #endregion

      #region Private Methods

      private void MultiTextFilterViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(MultiTextFilterViewModel.SearchText)))
         {
            DomainObjectsSearchViewModel.MultiTextFilterViewModel.RequestSearch();
         }
      }

      #endregion
   }
}
