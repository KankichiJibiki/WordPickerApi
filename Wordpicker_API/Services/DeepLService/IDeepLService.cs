using Wordpicker_API.DTOs;
using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.DeepLService
{
    public interface IDeepLService
    {
        Task<ApiResponse> GetTextsTranslated(TranslateRequestDto[] request);
        Task<ApiResponse> GetTextTranslated(TranslateRequestDto request);
    }
}
