using Newtonsoft.Json;
using System.Collections;
using System.Web;
using Wordpicker_API.Configs;
using Wordpicker_API.DTOs;
using Wordpicker_API.Services.DeepLService;
using Wordpicker_API.Services.HttpService;
using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.WordsApiService
{
    public class WordsApiService : IWordsApiService
    {
        private readonly int MAX_WORD_LENGTH = 30;

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

        public async Task<ApiResponse> GetWordFullInfoAsync(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                _response.SetResponse(false, StatusCodes.Status400BadRequest, "Word is blank", "");
                return _response;
            }

            if (word.Length > MAX_WORD_LENGTH)
            {
                _response.SetResponse(false, StatusCodes.Status400BadRequest, "Word is too long", "");
                return _response;
            }

            var encodedWord = System.Uri.EscapeDataString(word);
            var url = _config.GetWordsApiEndpoint() + $"words/{encodedWord}";
            try
            {
                var result = await _httpService.GetAsync(url, GetHeaders());

                if (result.GetResponse().StatusCode == StatusCodes.Status204NoContent || string.IsNullOrEmpty(result.GetResponse().Data))
                {
                    return result;
                }
                if (result.GetResponse().StatusCode != StatusCodes.Status200OK)
                {
                    throw new Exception($"Failed to Search the word {word}");
                }
                var finalResponse = await GetJPDefinitions(result.GetResponse().Data);

                result.SetResponse(true, StatusCodes.Status200OK, "", JsonConvert.SerializeObject(finalResponse));
                return result;
            } catch(Exception ex)
            {
                _response.SetResponse(true, StatusCodes.Status500InternalServerError, ex.Message, "");
                return _response;
            }
        }
        public async Task<ApiResponse> GetWordPronunciationCodeAsync(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                _response.SetResponse(false, StatusCodes.Status400BadRequest, "Word is blank", "");
                return _response;
            }

            if (word.Length > MAX_WORD_LENGTH)
            {
                _response.SetResponse(false, StatusCodes.Status400BadRequest, "Word is too long", "");
                return _response;
            }

            var encodedWord = System.Uri.EscapeDataString(word);
            var url = _config.GetWordsApiEndpoint() + $"words/{encodedWord}/pronunciation";
            try
            {
                var result = await _httpService.GetAsync(url, GetHeaders());
                if (result.GetResponse().StatusCode == StatusCodes.Status204NoContent || string.IsNullOrEmpty(result.GetResponse().Data))
                {
                    return result;
                }

                result.SetResponse(true, StatusCodes.Status200OK, "", result.GetResponse().Data);
                return result;
            }
            catch (Exception ex)
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
                    return root;
                }

                root.DefinitionsJp = JsonConvert.DeserializeObject<List<string>>(resultDefinitionJp.GetResponse().Data);
                return root;
            } catch(Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        private Dictionary<string, string> GetHeaders()
        {
            return new Dictionary<string, string>
                {
                    { "X-RapidAPI-Key", _config.GetWordsApiKey() },
                    { "X-RapidAPI-Host", _config.GetWordsApiHost() },
                };
        }
    }
}
