using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Services;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BusinessAppFramework.WebUI.Services
{
    public class ActionExecutionService : HttpService, IActionExecutionService
    {
        #region Fields

        private IUserDialogService _userDialogService;
        private ILocalizedStringService _localizedStringService;
        private NavigationManager _navigationManager;
        private IConfiguration _configuration;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public ActionExecutionService(
            IHttpClientFactory clientFactory,
            IOptions<ApiClientOptions> apiClientOptions,
            IUserDialogService userDialogService,
            ILocalizedStringService localizedStringService,
            NavigationManager navigationManager,
            IConfiguration configuration) :
            base(clientFactory, apiClientOptions)
        {
            _userDialogService = userDialogService;
            _localizedStringService = localizedStringService;
            _navigationManager = navigationManager;
            _configuration = configuration;
        }

        #endregion

        #region Public Methods        

        public async Task<DomainObjectApplicationActionResultPayload?> ExecuteApplicationActionAsync(IApplicationAction action)
        {
            var route = action.BuildRoute();

            if (action is IHttpDomainObjectApplicationAction httpApplicationAction)
            {
                HttpResult<DomainObjectApplicationActionResultPayload>? result = null;

                if (httpApplicationAction.HttpMethod == HttpMethod.Post)
                {
                    result = await PostAsync<DomainObjectApplicationActionResultPayload>(route, null);
                }
                else if (httpApplicationAction.HttpMethod == HttpMethod.Delete)
                {
                    if (await _userDialogService.ConfirmAsync(_localizedStringService.Get(MessageKeys.DeleteConfirmationMessage) ?? "delete_msg"))
                    {
                        result = await DeleteAsync<DomainObjectApplicationActionResultPayload>(route);
                    }
                }
                else if (httpApplicationAction.HttpMethod == HttpMethod.Put)

                {
                    result = await PutAsync<DomainObjectApplicationActionResultPayload>(route, null);
                }
                else if (httpApplicationAction.HttpMethod == HttpMethod.Get)
                {
                    result = await GetAsync<DomainObjectApplicationActionResultPayload>(route);
                }
                else
                {
                    throw new NotSupportedException($"Unsupported HTTP verb {httpApplicationAction.HttpMethod}");
                }

                return result?.Data ?? new DomainObjectApplicationActionResultPayload();
            }
            else if (action is IDomainObjectNavigationApplicationAction navigationApplicationAction)
            {
                _navigationManager.NavigateTo(route);
                return null;
            }
            else if (action is IDocumentDownloadApplicationAction documentDownloadApplicationAction)
            {
                var apiBaseUrl = _configuration["ApiBaseUrl"];
                var fullUrl = $"{apiBaseUrl}/{route}";

                _navigationManager.NavigateTo(fullUrl, forceLoad:true);
                return null;
            }
            else if (action is IDocumentNavigationApplicationAction documentNavigationApplicationAction)
            {              
                _navigationManager.NavigateTo(route);
                return null;
            }
            else
            {
                throw new ArgumentException($"Cannot handle {action.GetType()} action type");
            }
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
