using BusinessAppFramework.Application.Interfaces;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Net.Http.Json;
using System.Text.Json;

namespace BusinessAppFramework.WebUI.Services
{
    public abstract class HttpService
    {
        #region Fields

        protected readonly IHttpClientFactory _clientFactory;
        protected readonly ApiClientOptions _apiClientOptions;
        protected readonly ILogger _logger;
        protected readonly ILocalizedStringService _localizedStringService;
        protected readonly IUserDialogService _userDialogService;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public HttpService(
            IHttpClientFactory clientFactory, 
            IOptions<ApiClientOptions> apiClientOptions,
            ILogger<HttpService> logger,
            ILocalizedStringService localizedStringService,
            IUserDialogService userDialogService)
        {
            _clientFactory = clientFactory;
            _apiClientOptions = apiClientOptions.Value;
            _logger = logger;
            _localizedStringService = localizedStringService;
            _userDialogService = userDialogService;
        }

        #endregion

        #region Public Methods

        public async Task<HttpResult<T>> GetAsync<T>(string route)
        {
            return await TryExecuteHttpRequest<T>(client => client.GetAsync(route));
        }

        public async Task<HttpResult<T>> PostAsync<T>(string route, object? body)
        {
            return await TryExecuteHttpRequest<T>(client => client.PostAsJsonAsync(route, body));
        }

        public async Task<HttpResult<T>> PutAsync<T>(string route, object? body)
        {
            return await TryExecuteHttpRequest<T>(client => client.PutAsJsonAsync(route, body));
        }

        public async Task<HttpResult<T>> DeleteAsync<T>(string route)
        {
            return await TryExecuteHttpRequest<T>(client => client.DeleteAsync(route));
        }

        #endregion

        #region Private Methods

        protected HttpClient CreateClient() => _clientFactory.CreateClient(_apiClientOptions.ApiClientName);

        private async Task<T?> ReadFromJsonAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            return await httpResponseMessage.Content.ReadFromJsonAsync<T>(
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        private async Task<HttpResult<T>> TryExecuteHttpRequest<T>(Func<HttpClient, Task<HttpResponseMessage>> httpCall)
        {
            var result = await ExecuteHttpRequest<T>(httpCall);

            if (!result.Success)
            {
                await _userDialogService.DialogErrorAsync(result.ErrorMessage ?? "no error message");
                _logger.LogWarning("HTTP failure: {Report}", result.ErrorMessage);
            }

            return result;
        }

        private async Task<HttpResult<T>> ExecuteHttpRequest<T>(Func<HttpClient, Task<HttpResponseMessage>> httpCall)
        {
            try
            {
                var client = CreateClient();

                var response = await httpCall(client);

                if (!response.IsSuccessStatusCode)
                    return await CreateFailureHttpResultAsync<T>(response);

                return await CreateHttpResultAsync<T>(response);
            }
            catch (Exception ex)
            {
                return CreateExceptionHttpResult<T>(ex);
            }
        }

        private async Task<HttpResult<T>> CreateHttpResultAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            var data = await ReadFromJsonAsync<T>(httpResponseMessage);
            return new HttpResult<T>(true, data, null, (int)httpResponseMessage.StatusCode);
        }

        private async Task<HttpResult<T>> CreateFailureHttpResultAsync<T>(HttpResponseMessage response)
        {
            var body = response.Content != null ? await response.Content.ReadAsStringAsync() : null;

            var report = $"HTTP error {(int)response.StatusCode} : {response.ReasonPhrase}\n\n" +
                         $"Request message : \n{response.RequestMessage}\n\n" +
                         $"Body : \n{body}";

            return new HttpResult<T>(
                false,
                Data: default,
                ErrorMessage: report,
                StatusCode: (int)response.StatusCode);
        }        

        private HttpResult<T> CreateExceptionHttpResult<T>(Exception ex)
        {
            return new HttpResult<T>(
                Success: false,
                Data: default,
                ErrorMessage: ex.Message,
                StatusCode: null
            );
        }

        #endregion
    }
}
