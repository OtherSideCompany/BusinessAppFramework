using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BusinessAppFramework.WebUI.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        #region Fields

        private readonly IJSRuntime _jsRuntime;

        #endregion

        #region Constructor

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        #endregion

        #region Public Methods

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);

                if (string.IsNullOrEmpty(json))
                    return default;

                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex) when (ex is JSDisconnectedException or JsonException)
            {
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
            }
            catch (JSDisconnectedException)
            {
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
            }
            catch (JSDisconnectedException)
            {
            }
        }

        public async Task ClearAsync()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.clear");
            }
            catch (JSDisconnectedException)
            {
            }
        }

        #endregion
    }
}
