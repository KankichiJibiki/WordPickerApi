using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.WordsApiService
{
    public interface IWordsApiService
    {
        Task<ApiResponse> GetWordFullInfoAsync(string word);
        Task<ApiResponse> GetWordPronunciationCodeAsync(string word);
    }
}
