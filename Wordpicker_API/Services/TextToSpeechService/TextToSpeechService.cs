using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.TextToSpeechService
{
    public class TextToSpeechService : ITextToSpeechService
    {
        private readonly IConfiguration _config;
        private readonly ApiResponse _response;

        public TextToSpeechService(IConfiguration config) 
        {
            _config = config;
            _response = new ApiResponse();
        }
        public async Task<ApiResponse> ConvertTextToSpeech(string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                _response.SetResponse(false, StatusCodes.Status400BadRequest, "Text was either null or empty", "");
                return _response;
            }


        }
    }
}
