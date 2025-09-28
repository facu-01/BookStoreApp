
namespace BookStoreApp.Blazor.WebAssembly.UI.Services.LocalStorage
{
    public interface ILocalStorageService
    {
        Task ClearAsync();
        Task<string> GetItemAsync(string key);
        Task RemoveItemAsync(string key);
        Task SetItemAsync(string key, string value);
    }
}