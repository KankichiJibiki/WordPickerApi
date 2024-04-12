using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Wordpicker_API.Configs;
using Wordpicker_API.DTOs;
using Wordpicker_API.Services.DeepLService;
using Wordpicker_API.Services.HttpService;
using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.WordsApiService
{
    public class WordsApiService : IWordsApiService
    {
        private readonly IHttpService _httpService;
        private readonly IDeepLService _deepLService;
        private readonly IAppConfigs _config;
        private readonly ApiResponse _response;

        public WordsApiService(IHttpService httpService, IAppConfigs config, IDeepLService deepLService) 
        {
            _httpService = httpService;
            _deepLService = deepLService;
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

                var result = await _httpService.GetAsync(url, headers);
                if (result.GetResponse().StatusCode == StatusCodes.Status204NoContent || string.IsNullOrEmpty(result.GetResponse().Data))
                {
                    return result;
                }

                var finalResponse = await GetJPDefinitions(result.GetResponse().Data);
                if(finalResponse.DefinitionsJp == null)
                {
                    return result;
                }

                result.SetResponse(true, StatusCodes.Status200OK, "Got Jp definitions as well", JsonConvert.SerializeObject(finalResponse));
                return result;
            } catch(Exception ex)
            {
                _response.SetResponse(true, StatusCodes.Status500InternalServerError, ex.Message, "");
                return _response;
            }
        }

        private async Task<Root> GetJPDefinitions(string jsonData)
        {
            Root root = JsonConvert.DeserializeObject<Root>(jsonData);

            if(root.Results.Count == 0)
            {
                return root;
            }

            List<TranslateRequestDto> translateRequests = new List<TranslateRequestDto>();
            foreach(var result in root.Results)
            {
                translateRequests.Add(new TranslateRequestDto { Text = result.Definition, EnToJp = true });
            }

            try
            {
                var resultDefinitionJp = await _deepLService.GetTextsTranslated(translateRequests.ToArray());
                if(!resultDefinitionJp.GetResponse().Success)
                {
                    throw new InvalidDataException();
                }

                root.DefinitionsJp = JsonConvert.DeserializeObject<List<string>>(resultDefinitionJp.GetResponse().Data);

                return root;
            } catch(Exception ex)
            {
                throw new InvalidDataException();
            }
        }
    }
}
