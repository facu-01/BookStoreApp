namespace BookStoreApp.Blazor.Server.UI.Services.Http
{
    public interface IBaseHttpClient
    {
        Task<Response<T>> MakeRequest<T>(Func<IClient, Task<T>> request);

        Task<Response> MakeRequest(Func<IClient, Task> request);

    }
}
