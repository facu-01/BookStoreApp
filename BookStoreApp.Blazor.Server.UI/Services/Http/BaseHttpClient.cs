


using System.Net;
using System.Net.Http.Headers;
using BookStoreApp.Blazor.Server.UI.Services.LocalStorage;

namespace BookStoreApp.Blazor.Server.UI.Services.Http
{
    public class BaseHttpClient : Client, IBaseHttpClient
    {
        private readonly ILocalStorageService _localStorageService;

        public BaseHttpClient(HttpClient httpClient, ILocalStorageService localStorageService) : base(httpClient)
        {
            _localStorageService = localStorageService;
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
