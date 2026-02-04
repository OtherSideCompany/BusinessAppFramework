using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Search;

namespace OtherSideCore.Adapter.Factories
{
   public class DomainObjectSearchViewModelFactory : TypeBasedFactory, IDomainObjectsSearchViewModelFactory
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor



      #endregion

      #region Public Methods

      public void RegisterDomainObjectSearchViewModel<TSearchResult>(Func<IDomainObjectSearch<TSearchResult>, IDomainObjectSearchViewModel> factory) where TSearchResult : DomainObjectSearchResult, new()
      {
         Register<TSearchResult>(args =>
         {
            var search = (IDomainObjectSearch<TSearchResult>)args[0]!;
            return factory(search);
         });
      }

      public IDomainObjectSearchViewModel CreateDomainObjectSearchViewModel<TSearchResult>(IDomainObjectSearch<TSearchResult> domainObjectSearch) where TSearchResult : DomainObjectSearchResult, new()
      {
         return (IDomainObjectSearchViewModel)CreateFromType(typeof(TSearchResult), domainObjectSearch);
      }      

      #endregion

      #region Private Methods



      #endregion

   }
}
