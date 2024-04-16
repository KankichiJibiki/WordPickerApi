using Wordpicker_API.DTOs;
using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.TextToSpeechService
{
    public interface ITextToSpeechService
    {
        Task<ApiResponse> ConvertTextToSpeech(TextToSpeechRequestDto request);
    }
}
