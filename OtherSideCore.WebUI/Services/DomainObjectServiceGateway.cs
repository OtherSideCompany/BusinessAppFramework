using Microsoft.Extensions.Options;
using OtherSideCore.Contracts;
using OtherSideCore.Contracts.ActionResult;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.WebUI.Interfaces;

namespace OtherSideCore.WebUI.Services
{
    public class DomainObjectServiceGateway<T> : HttpService, IDomainObjectServiceGateway<T> where T : DomainObject, new()
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public DomainObjectServiceGateway(IHttpClientFactory clientFactory, IOptions<ApiClientOptions> apiClientOptions)
            : base(clientFactory, apiClientOptions)
        {

        }

        #endregion

        #region Public Methods

        public async Task<T?> CreateAsync()
        {
            var result = await PostAsync<DomainObjectApplicationActionResultPayload>(Routes.BuildRoute(Routes.CreateTemplate, typeof(T), null), null);

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
            return (await GetAsync<T>(Routes.BuildRoute(Routes.GetTemplate, typeof(T), domainObjectId))).Data;
        }

        public async Task<List<int>> GetChildrenIdsAsync(int parentId, string key, CancellationToken cancellationToken = default)
        {
            return (await GetAsync<List<int>>(Routes.BuildRoute(Routes.GetChildrenTemplate, typeof(T), parentId, key))).Data ?? new List<int>();
        }

        public async Task<T?> GetHydratedAsync(int domainObjectId, CancellationToken cancellationToken = default)
        {
            return (await GetAsync<T>(Routes.BuildRoute(Routes.GetHydratedTemplate, typeof(T), domainObjectId))).Data;
        }

        public async Task<DomainObjectReference?> GetHydratedDomainObjectReference(int domainObjectId, string key)
        {
            return (await GetAsync<DomainObjectReference>(Routes.BuildRoute(Routes.GetHydratedDomainObjectReferenceTemplate, typeof(T), domainObjectId, key))).Data;
        }

        public async Task<DomainObjectReferenceListItem?> GetHydratedDomainObjectReferenceListItem(int domainObjectReferenceListItemId, string key)
        {
            return (await GetAsync<DomainObjectReferenceListItem>(Routes.BuildRoute(Routes.GetHydratedDomainObjectReferenceListItemTemplate, typeof(T), domainObjectReferenceListItemId, key))).Data;
        }       

        public async Task SaveAsync(T domainObject, CancellationToken cancellationToken = default)
        {
            await PutAsync<T>(Routes.BuildRoute(Routes.SaveTemplate, typeof(T), domainObject.Id), domainObject);
        }

        public async Task DeleteAsync(int domainObjectId)
        {
            await DeleteAsync<T>(Routes.BuildRoute(Routes.DeleteTemplate, typeof(T), domainObjectId));
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
