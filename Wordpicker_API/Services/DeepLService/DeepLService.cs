using DeepL;
using System.Text.Json;
using Wordpicker_API.Configs;
using Wordpicker_API.DTOs;
using Wordpicker_API.Services.HttpService;
using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.DeepLService
{
    public class DeepLService : IDeepLService
    {
        private readonly IHttpService _httpService;
        private readonly IAppConfigs _config;
        private readonly ApiResponse _response;
        private readonly Translator _deepLTranslator;

        public DeepLService(IHttpService httpService, IAppConfigs config)
        {
            _httpService = httpService;
            _config = config;
            _response = new ApiResponse();
            _deepLTranslator = new Translator(_config.GetDeepLApiKey());
        }

        public async Task<ApiResponse> GetTextsTranslated(TranslateRequestDto[] request)
        {
            if(request.Length == 0)
            {
                _response.SetResponse(false, StatusCodes.Status400BadRequest, "Word is blank", "");
                return _response;
            }

            var translatedTexts = new List<string>();
            try
            {
                foreach (var text in request)
                {
                    var translatedText = await GetTextTranslated(text);
                    if(!translatedText.GetResponse().Success)
                    {
                        throw new InvalidDataException("Failed to translate");
                    }
                    translatedTexts.Add(translatedText.GetResponse().Data);
                }

                _response.SetResponse(true, StatusCodes.Status200OK, "All texts turned into your language", JsonSerializer.Serialize(translatedTexts));
                return _response;
            } catch(Exception ex)
            {
                _response.SetResponse(false, StatusCodes.Status500InternalServerError, ex.Message, "");
                return _response;
            }
        }

        public async Task<ApiResponse> GetTextTranslated(TranslateRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Text))
            {
                _response.SetResponse(false, StatusCodes.Status400BadRequest, "Word is blank", "");
                return _response;
            }

            var translateFrom = request.EnToJp ? "en" : "ja";
            var translateTo = request.EnToJp ? "ja" : "en";

            try
            {
                var translatedText = await _deepLTranslator.TranslateTextAsync(
                    request.Text,
                    translateFrom,
                    translateTo
                );

                if (translatedText == null)
                {
                    throw new InvalidDataException("Failed to translate");
                }

                _response.SetResponse(true, StatusCodes.Status200OK, "Translated Successfully", translatedText.ToString());
                return _response;
            } catch ( Exception ex )
            {
                _response.SetResponse(false, StatusCodes.Status500InternalServerError, ex.Message, "");
                return _response;
            }
        }
    }
}
