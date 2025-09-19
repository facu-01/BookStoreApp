using Microsoft.JSInterop;

namespace BookStoreApp.Blazor.Server.UI.Services.LocalStorage
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jSRuntime;

        public LocalStorageService(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        public async Task SetItemAsync(string key, string value)
        {
            await _jSRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }

        public async Task<string> GetItemAsync(string key)
        {
            return await _jSRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }


        public async Task RemoveItemAsync(string key)
        {
            await _jSRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public async Task ClearAsync()
        {
            await _jSRuntime.InvokeVoidAsync("localStorage.clear");
        }

    }
}
