using BookStoreApp.Blazor.WebAssembly.UI.Providers;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Http;
using BookStoreApp.Blazor.WebAssembly.UI.Services.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _apiAuthenticationStateProvider;

        public AuthenticationService(IClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _apiAuthenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> AuthenticateAsync(UserLoginDto userLoginDto)
        {
            var response = await _httpClient.LoginAsync(userLoginDto);

            // store token
            await _localStorageService.SetItemAsync("accessToken", response.Token);

            // change auth state
            await ((ApiAuthenticationStateProvider)_apiAuthenticationStateProvider).LoggedIn();

            return true;

        }

        public async Task Logout()
        {
            await ((ApiAuthenticationStateProvider)_apiAuthenticationStateProvider).LoggedOut();
        }
    }
}
