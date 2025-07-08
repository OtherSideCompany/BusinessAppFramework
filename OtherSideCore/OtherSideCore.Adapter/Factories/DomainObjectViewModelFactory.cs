using OtherSideCore.Application.Factories;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter.Factories
{
   public class DomainObjectViewModelFactory : TypeBasedFactory, IDomainObjectViewModelFactory
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

      public void RegisterViewModel<T>(Func<DomainObject, DomainObjectViewModel> factory) where T : DomainObject, new()
      {
         Register<T>(args =>
         {
            var domainObject = (DomainObject)args[0]!;
            return factory(domainObject);
         });
      }

      public async Task<DomainObjectViewModel> CreateViewModelAsync(DomainObject domainObject)
      {
         var viewModel = (DomainObjectViewModel)CreateFromType(domainObject.GetType(), domainObject);
         viewModel.InitializeProperties();
         await viewModel.InitializeSearchResultsAsync();
         return viewModel;
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
