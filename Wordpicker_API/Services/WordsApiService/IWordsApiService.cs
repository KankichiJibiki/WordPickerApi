using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.WordsApiService
{
    public interface IWordsApiService
    {
        Task<ApiResponse> GetWordAsync(string word);
    }
}
