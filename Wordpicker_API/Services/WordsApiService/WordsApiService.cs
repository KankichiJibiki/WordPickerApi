using Newtonsoft.Json;
using Wordpicker_API.Configs;
using Wordpicker_API.Services.HttpService;
using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.WordsApiService
{
    public class WordsApiService : IWordsApiService
    {
        private readonly IHttpService _httpService;
        private readonly IAppConfigs _config;
        private readonly ApiResponse _response;

        public WordsApiService(IHttpService httpService, IAppConfigs config) 
        {
            _httpService = httpService;
            _config = config;
            _response = new ApiResponse();
        }

        public async Task<ApiResponse> GetWordAsync(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                _response.SetResponse(false, StatusCodes.Status400BadRequest, "Word is blank", "");
                return _response;
            }

            var url = _config.GetWordsApiEndpoint() + $"words/{word}";
            try
            {
                var headers = new Dictionary<string, string>
                {
                    { "X-RapidAPI-Key", _config.GetWordsApiKey() },
                    { "X-RapidAPI-Host", _config.GetWordsApiHost() },
                };

                return await _httpService.GetAsync(url, headers);
            } catch(Exception ex)
            {
                _response.SetResponse(true, StatusCodes.Status500InternalServerError, ex.Message, "");
                return _response;
            }
        }
    }
}
