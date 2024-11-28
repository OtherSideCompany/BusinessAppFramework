using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Application.DomainObjectBrowser;
using OtherSideCore.Appplication.Services;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public class DomainObjectTreeSearchViewModel : ObservableObject, IDomainObjectTreeSearchViewModel
   {
      #region Fields

      protected DomainObjectTreeSearch _domainObjectTreeSearch;
      protected DomainObjectViewModel _parentViewModel;

      private bool _isExecutingSearch;

      #endregion

      #region Properties

      public bool IsExecutingSearch
      {
         get => _isExecutingSearch;
         set => SetProperty(ref _isExecutingSearch, value);
      }


      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectTreeSearchViewModel(DomainObjectTreeSearch domainObjectTreeSearch,
                                             IUserDialogService userDialogService,
                                             IWindowService windowService,
                                             IDomainObjectInteractionFactory domainObjectInteractionFactory)
      {
         _domainObjectTreeSearch = domainObjectTreeSearch;
      }

      #endregion

      #region Public Methods

      public async Task SearchAsync(DomainObjectViewModel domainObjectViewModel)
      {
         IsExecutingSearch = true;

         UnloadSearchResultViewModels();

         await _domainObjectTreeSearch.SearchAsync(domainObjectViewModel.DomainObject);

         LoadSearchResultViewModels();

         IsExecutingSearch = false;
      }

      public void LoadSearchResultViewModels()
      {
         
      }      

      public void UnloadSearchResultViewModels()
      {
         
      }

      public void Dispose()
      {
         UnloadSearchResultViewModels();
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
