using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MudBlazor;

namespace BusinessAppFramework.WebUI.Services
{
    public class ApplicationActionExecutionService : HttpService, IApplicationActionExecutionService
    {
        #region Fields

        private NavigationManager _navigationManager;
        private IConfiguration _configuration;
        private IDialogService _dialogService;
        private IComponentRegistry _componentRegistry;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public ApplicationActionExecutionService(
            IHttpClientFactory clientFactory,
            IOptions<ApiClientOptions> apiClientOptions,
            IUserDialogService userDialogService,
            ILocalizedStringService localizedStringService,
            NavigationManager navigationManager,
            IConfiguration configuration,
            ILogger<ApplicationActionExecutionService> logger,
            IDialogService dialogService,
            IComponentRegistry componentRegistry) :
            base(clientFactory, apiClientOptions, logger, localizedStringService, userDialogService)
        {
            _navigationManager = navigationManager;
            _configuration = configuration;
            _dialogService = dialogService;
            _componentRegistry = componentRegistry;
        }

        #endregion

        #region Public Methods        

        public async Task<DomainObjectApplicationActionResultPayload?> ExecuteApplicationActionAsync(IApplicationAction action)
        {
            var route = action.BuildRoute();

            if (action is IHttpDomainObjectApplicationAction httpApplicationAction)
            {
                HttpResult<DomainObjectApplicationActionResultPayload>? result = null;

                if (httpApplicationAction.ActionKey.Key == ActionKeys.ImportExportDataActionKey)
                {
                    _userDialogService.SnackShow(_localizedStringService.Get(MessageKeys.NotImplementedMessage)!);
                    return new DomainObjectApplicationActionResultPayload();
                }

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
            else if (action is IDomainObjectNavigationApplicationAction domainObjectNavigationApplicationAction)
            {
                _navigationManager.NavigateTo(route);
                return null;
            }
            else if (action is IOpenDialogApplicationAction openDialogApplicationAction)
            {
                DialogParameters? parameters = new DialogParameters
                {
                    { nameof(IOpenDialogApplicationAction.ExecuteRoute), openDialogApplicationAction.ExecuteRoute }
                };

                var dialogOptions = new DialogOptions
                {
                    MaxWidth = MaxWidth.Medium,
                    FullWidth = true
                };

                var componentType = _componentRegistry.Resolve(openDialogApplicationAction.ComponentKey);
                var dialog = await _dialogService.ShowAsync(componentType, openDialogApplicationAction.DialogTitle, parameters, dialogOptions);
                var result = await dialog.Result;

                if (result != null && result.Data is DomainObjectApplicationActionResultPayload domainObjectApplicationActionResultPayload)
                    return domainObjectApplicationActionResultPayload;

                return null;
            }
            else if (action is IDocumentDownloadApplicationAction documentDownloadApplicationAction)
            {
                var apiBaseUrl = _configuration["ApiBaseUrl"];
                var fullUrl = $"{apiBaseUrl}/{route}";

                _navigationManager.NavigateTo(fullUrl, forceLoad: true);
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
