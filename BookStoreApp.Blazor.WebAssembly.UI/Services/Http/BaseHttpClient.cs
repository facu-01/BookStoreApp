


using System.Net;
using System.Net.Http.Headers;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Authentication;
using BookStoreApp.Blazor.WebAssembly.UI.Services.LocalStorage;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Http
{
    public class BaseHttpClient : Client, IBaseHttpClient
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IAuthenticationService _authenticationService;

        public BaseHttpClient(HttpClient httpClient, ILocalStorageService localStorageService, IAuthenticationService authenticationService) : base(httpClient)
        {
            _localStorageService = localStorageService;
            _authenticationService = authenticationService;
        }

        public async Task<Response<T>> MakeRequest<T>(Func<IClient, Task<T>> request)
        {
            try
            {
                await SetBearerToken();

                var response = await request(this);

                return new Response<T>
                {
                    Data = response,
                    Success = true,
                };
            }
            catch (ApiException ex)
            {

                var response = ConvertApiException(ex);

                if (ex.StatusCode == ((int)HttpStatusCode.Unauthorized))
                {
                    await _authenticationService.Logout();
                }

                return new Response<T>
                {
                    Message = response.Message,
                    Success = response.Success,
                    ValidationErrors = response.ValidationErrors,
                };

            }
        }

        public async Task<Response> MakeRequest(Func<IClient, Task> request)
        {
            try
            {
                await SetBearerToken();

                await request(this);

                return new Response
                {
                    Success = true,
                };
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == ((int)HttpStatusCode.Unauthorized))
                {
                    await _authenticationService.Logout();
                }

                return ConvertApiException(ex);

            }
        }

        private async Task SetBearerToken()
        {
            var token = await _localStorageService.GetItemAsync("accessToken");

            if (token == null) return;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        }


        private static Response ConvertApiException(ApiException ex) => ex.StatusCode switch
        {
            (int)HttpStatusCode.BadRequest => new Response
            {
                Message = "Validation errors have ocurred.",
                Success = false,
                ValidationErrors = ex.Response
            },
            (int)HttpStatusCode.NotFound => new Response
            {
                Message = "The request resource doesnt exists.",
                Success = false,
            },
            (int)HttpStatusCode.Forbidden => new Response
            {
                Message = "You dont have permission to do that.",
                Success = false,
            },
            _ => new Response
            {
                Message = "Something went wrong, please try again.",
                Success = false,
            },
        };


    }
}
