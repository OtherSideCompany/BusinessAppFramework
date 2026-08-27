using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using MudBlazor;

namespace BusinessAppFramework.WebUI.Services
{
    public class ApplicationActionExecutionService : HttpService, IApplicationActionExecutionService
    {
        #region Fields

        private NavigationManager _navigationManager;
        private IDialogService _dialogService;
        private IComponentRegistry _componentRegistry;
        private IJSRuntime _jsRuntime;

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
            ILogger<ApplicationActionExecutionService> logger,
            IDialogService dialogService,
            IComponentRegistry componentRegistry,
            IJSRuntime jsRuntime) :
            base(clientFactory, apiClientOptions, logger, localizedStringService, userDialogService)
        {
            _navigationManager = navigationManager;
            _dialogService = dialogService;
            _componentRegistry = componentRegistry;
            _jsRuntime = jsRuntime;
        }

        #endregion

        #region Public Methods        

        public async Task<DomainObjectApplicationActionResultPayload?> ExecuteApplicationActionAsync(IApplicationAction action)
        {
            var route = action.BuildRoute();

            return action switch
            {
                IHttpDomainObjectApplicationAction httpApplicationAction => await ExecuteHttpApplicationActionAsync(httpApplicationAction, route),
                IDomainObjectNavigationApplicationAction => ExecuteNavigationApplicationAction(route),
                IOpenDialogApplicationAction openDialogApplicationAction => await ExecuteOpenDialogApplicationActionAsync(openDialogApplicationAction),
                IFileDownloadApplicationAction => await ExecuteFileDownloadApplicationActionAsync(route),
                IDocumentNavigationApplicationAction => ExecuteNavigationApplicationAction(route),
                _ => throw new ArgumentException($"Cannot handle {action.GetType()} action type")
            };
        }

        #endregion

        #region Private Methods

        private async Task<DomainObjectApplicationActionResultPayload?> ExecuteHttpApplicationActionAsync(IHttpDomainObjectApplicationAction httpApplicationAction, string route)
        {
            if (httpApplicationAction.ActionKey == ActionKeys.ImportExportDataActionKey)
            {
                _userDialogService.SnackShow(_localizedStringService.Get(MessageKeys.NotImplementedMessage)!);
                return new DomainObjectApplicationActionResultPayload();
            }

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

        private DomainObjectApplicationActionResultPayload? ExecuteNavigationApplicationAction(string route)
        {
            _navigationManager.NavigateTo(route);
            return null;
        }

        private async Task<DomainObjectApplicationActionResultPayload?> ExecuteOpenDialogApplicationActionAsync(IOpenDialogApplicationAction openDialogApplicationAction)
        {
            DialogParameters? parameters = new DialogParameters
            {
                { nameof(IOpenDialogApplicationAction.ExecuteRoute), openDialogApplicationAction.ExecuteRoute },
                { nameof(IOpenDialogApplicationAction.DomainObjectId), openDialogApplicationAction.DomainObjectId }
            };

            foreach (var keyValuePair in openDialogApplicationAction.AdditionalParameters)
                parameters.Add(keyValuePair.Key, keyValuePair.Value);

            var dialogOptions = new DialogOptions
            {
                MaxWidth = MaxWidth.False,
                FullWidth = false
            };

            var componentType = _componentRegistry.Resolve(openDialogApplicationAction.ComponentKey);
            var dialog = await _dialogService.ShowAsync(componentType, openDialogApplicationAction.DialogTitle, parameters, dialogOptions);
            var result = await dialog.Result;

            if (result != null && result.Data is DomainObjectApplicationActionResultPayload domainObjectApplicationActionResultPayload)
                return domainObjectApplicationActionResultPayload;

            return null;
        }

        private async Task<DomainObjectApplicationActionResultPayload?> ExecuteFileDownloadApplicationActionAsync(string route)
        {
            var response = await CreateClient().GetAsync(route);
            response.EnsureSuccessStatusCode();

            var bytes = await response.Content.ReadAsByteArrayAsync();
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";
            var fileName = response.Content.Headers.ContentDisposition?.FileNameStar
                ?? response.Content.Headers.ContentDisposition?.FileName?.Trim('"')
                ?? "download";

            await _jsRuntime.InvokeVoidAsync("oscDownload", fileName, contentType, Convert.ToBase64String(bytes));
            return null;
        }

        #endregion
    }
}
