using BookStoreApp.Blazor.Server.UI.Services.Http;

namespace BookStoreApp.Blazor.Server.UI.Services.Authentication
{
    public interface IAuthenticationService
    {
        public Task<bool> AuthenticateAsync(UserLoginDto userLoginDto);

        public Task Logout();
    }
}
