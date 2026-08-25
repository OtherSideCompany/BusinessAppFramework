using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.Domain.DomainObjects;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BusinessAppFramework.WebUI.Services
{
    public class DomainObjectServiceGateway<T> : HttpService, IDomainObjectServiceGateway<T> where T : DomainObject, new()
    {
        #region Fields
        protected string _baseUrl => ApiRoute.DomainObjectControllerRoute<T>();

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public DomainObjectServiceGateway(
            IHttpClientFactory clientFactory,
            IOptions<ApiClientOptions> apiClientOptions,
            ILogger<DomainObjectServiceGateway<T>> logger,
            ILocalizedStringService localizedStringService,
            IUserDialogService userDialogService)
          : base(clientFactory, apiClientOptions, logger, localizedStringService, userDialogService)
        {
            
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

        public async Task<DomainObjectApplicationActionResultPayload> CreateAsync(T domainObject)
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.CreateFromDomainObject}";
            var result = await PostAsync<DomainObjectApplicationActionResultPayload>(route, domainObject);

            if (result != null && result.Data != null && result.Data.Changes.Any(c => c.ChangeType == ChangeType.Added))
            {
                domainObject.Id = result.Data.Changes.First(c => c.ChangeType == ChangeType.Added).DomainObjectId;
            }

            return result?.Data ?? new DomainObjectApplicationActionResultPayload() { ErrorMessageKey = Contracts.MessageKeys.ServerError };
        }

        public async Task<T?> GetAsync(int domainObjectId, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.Get}/{domainObjectId}";
            return (await GetAsync<T>(route)).Data;
        }

        public async Task<T?> GetOrDefaultAsync(int domainObjectId, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.GetOrDefault}/{domainObjectId}";
            return (await GetAsync<T>(route)).Data;
        }

        public async Task<List<T>> GetAllAsync(List<int> domainObjectIds, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.GetAll}";
            return (await PostAsync<List<T>>(route, domainObjectIds)).Data ?? new List<T>();
        }

        public async Task<T?> GetHydratedAsync(int domainObjectId, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.GetHydrated}/{domainObjectId}";
            return (await GetAsync<T>(route)).Data;
        }

        public async Task<List<T>> GetAllHydratedAsync(List<int> domainObjectIds, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.GetAllHydrated}";
            return (await PostAsync<List<T>>(route, domainObjectIds)).Data ?? new List<T>();
        }        

        public async Task<DomainObjectApplicationActionResultPayload> SaveAsync(T domainObject, CancellationToken cancellationToken = default)
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.Save}";
            var result = await PutAsync<DomainObjectApplicationActionResultPayload>(route, domainObject);
            return result?.Data ?? BuildServerErrorPayload();
        }

        public async Task<DomainObjectApplicationActionResultPayload> DeleteAsync(int domainObjectId)
        {
            var route = $"{_baseUrl}/{DomainObjectRouteSegments.Delete}/{domainObjectId}";
            var result = await DeleteAsync<DomainObjectApplicationActionResultPayload>(route);
            return result?.Data ?? BuildServerErrorPayload();
        }

        protected static DomainObjectApplicationActionResultPayload BuildServerErrorPayload()
            => new() { ErrorMessageKey = Contracts.MessageKeys.ServerError };

        #endregion

        #region Private Methods



        #endregion
    }
}
