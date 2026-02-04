using Azure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using OtherSideCore.Application;
using OtherSideCore.Application.Search;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace OtherSideCore.WebUI.Services
{
    public abstract class HttpService
    {
        #region Fields

        private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

        protected readonly IHttpClientFactory _clientFactory;
        protected readonly ApiClientOptions _apiClientOptions;

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public HttpService(IHttpClientFactory clientFactory, IOptions<ApiClientOptions> apiClientOptions)
        {
            _clientFactory = clientFactory;
            _apiClientOptions = apiClientOptions.Value;
        }

        #endregion

        #region Public Methods

        public async Task<HttpResult<T>> GetAsync<T>(string route)
        {
            return await ExecuteHttpRequest<T>(client => client.GetAsync(route));
        }

        public async Task<HttpResult<T>> PostAsync<T>(string route, object? body)
        {
            return await ExecuteHttpRequest<T>(client => client.PostAsJsonAsync(route, body));
        }

        public async Task<HttpResult<T>> PutAsync<T>(string route, object? body)
        {
            return await ExecuteHttpRequest<T>(client => client.PutAsJsonAsync(route, body));
        }

        public async Task<HttpResult<T>> DeleteAsync<T>(string route)
        {
            return await ExecuteHttpRequest<T>(client => client.DeleteAsync(route));
        }

        #endregion

        #region Private Methods

        private HttpClient CreateClient() => _clientFactory.CreateClient(_apiClientOptions.ApiClientName);

        private async Task<T?> ReadFromJsonAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            return await httpResponseMessage.Content.ReadFromJsonAsync<T>(
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
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

        private async Task<HttpResult<T>> CreateFailureHttpResultAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            string? error = null;

            try
            {
                error = await httpResponseMessage.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(error))
                    error = httpResponseMessage.ReasonPhrase;
            }
            catch
            {
                error = httpResponseMessage.ReasonPhrase;
            }

            return new HttpResult<T>(
                Success: false,
                Data: default,
                ErrorMessage: error,
                StatusCode: (int)httpResponseMessage.StatusCode
            );
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
