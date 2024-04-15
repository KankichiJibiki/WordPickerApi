using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.TextToSpeechService
{
    public interface ITextToSpeechService
    {
        Task<ApiResponse> ConvertTextToSpeech(string text);
    }
}
