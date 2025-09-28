using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookStoreApp.Blazor.WebAssembly.UI.Services.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.WebAssembly.UI.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        public ApiAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var notLoggedIn = new ClaimsPrincipal(new ClaimsIdentity());

            var token = await _localStorageService.GetItemAsync("accessToken");

            if (token == null)
            {
                return new AuthenticationState(notLoggedIn);
            }

            var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(token);

            if (tokenContent.ValidTo < DateTime.UtcNow)
            {
                await _localStorageService.RemoveItemAsync("accessToken");
            }

            var claims = GetClaims(tokenContent);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            return new AuthenticationState(user);

        }

        public async Task LoggedIn()
        {
            var token = await _localStorageService.GetItemAsync("accessToken");
            if (token == null) return;
            var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(token);
            var claims = GetClaims(tokenContent);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }


        public async Task LoggedOut()
        {
            await _localStorageService.RemoveItemAsync("accessToken");
            var notLoggedIn = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(notLoggedIn));
            NotifyAuthenticationStateChanged(authState);
        }

        private static List<Claim> GetClaims(JwtSecurityToken token)
        {
            var claims = token.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, token.Subject));
            return claims;
        }

    }
}
