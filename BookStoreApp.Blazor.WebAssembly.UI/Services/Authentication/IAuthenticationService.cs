using BookStoreApp.Blazor.WebAssembly.UI.Services.Http;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Authentication
{
    public interface IAuthenticationService
    {
        public Task<bool> AuthenticateAsync(UserLoginDto userLoginDto);

        public Task Logout();
    }
}
