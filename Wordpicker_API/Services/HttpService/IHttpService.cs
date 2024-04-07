using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.HttpService
{
    public interface IHttpService
    {
        Task<ApiResponse> GetAsync(string url, IDictionary<string, string>? headers);
    }
}
