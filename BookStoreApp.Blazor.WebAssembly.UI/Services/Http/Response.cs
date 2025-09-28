namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Http
{

    public class Response
    {
        public string Message { get; set; }

        public string ValidationErrors { get; set; }

        public bool Success { get; set; }

    }

    public class Response<T> : Response
    {
        public T? Data { get; set; }
    }


}
