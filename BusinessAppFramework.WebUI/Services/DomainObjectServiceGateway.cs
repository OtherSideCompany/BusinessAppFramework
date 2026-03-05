using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain.DomainObjects;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Options;

namespace BusinessAppFramework.WebUI.Services
{
    public class DomainObjectServiceGateway<T> : HttpService, IDomainObjectServiceGateway<T> where T : DomainObject, new()
    {
        #region Fields

        private string _controllerKey => _domainObjectRouteKeyRegistry.GetRouteKey<T>();
        private string _baseUrl => $"{ApiRouteSegments.Root}/{ApiRouteSegments.DomainObjects}/{_controllerKey}";

        private IDomainObjectRouteKeyRegistry _domainObjectRouteKeyRegistry;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public DomainObjectServiceGateway(
            IHttpClientFactory clientFactory, 
            IOptions<ApiClientOptions> apiClientOptions,
            IDomainObjectRouteKeyRegistry domainObjectRouteKeyRegistry)
          : base(clientFactory, apiClientOptions)
        {
            _domainObjectRouteKeyRegistry = domainObjectRouteKeyRegistry;
        }

        #endregion

        #region Public Methods

        public async Task<T?> CreateAsync()
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.Create}";
            var result = await PostAsync<DomainObjectApplicationActionResultPayload>(route, null);

            if (result != null && result.Data != null && result.Data.Changes.Any(c => c.ChangeType == ChangeType.Added))
            {
                return await GetAsync(result.Data.Changes.First(c => c.ChangeType == ChangeType.Added).DomainObjectId);
            }
            else
            {
                return null;
            }
        }

        public async Task<T?> GetAsync(int domainObjectId, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.Get}/{domainObjectId}";
            return (await GetAsync<T>(route)).Data;
        }

        public async Task<T?> GetHydratedAsync(int domainObjectId, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.GetHydrated}/{domainObjectId}";
            return (await GetAsync<T>(route)).Data;
        }

        public async Task<DomainObjectReference?> GetHydratedDomainObjectReference(int domainObjectId, string relationKeyReference)
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.GetHydratedReference}/{domainObjectId}/{relationKeyReference}";
            return (await GetAsync<DomainObjectReference>(route)).Data;
        }

        public async Task<DomainObjectReferenceListItem?> GetHydratedDomainObjectReferenceListItem(int domainObjectReferenceListItemId, string relationKeyReference)
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.GetHydratedReferenceList}/{domainObjectReferenceListItemId}/{relationKeyReference}";
            return (await GetAsync<DomainObjectReferenceListItem>(route)).Data;
        }

        public async Task SaveAsync(T domainObject, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.Save}";
            await PutAsync<T>(route, domainObject);
        }

        public async Task DeleteAsync(int domainObjectId)
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.Delete}/{domainObjectId}";
            await DeleteAsync<T>(route);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
