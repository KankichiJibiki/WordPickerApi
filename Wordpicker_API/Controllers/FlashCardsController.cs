using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Wordpicker_API.DTOs;
using Wordpicker_API.Services.TextToSpeechService;
using Wordpicker_API.Services.WordsApiService;

namespace Wordpicker_API.Controllers
{
    [ApiController]
    [Route("wordpickerapi/[controller]/[action]")]
    public class FlashCardsController : Controller
    {
        private readonly IWordsApiService _wordsApiService;
        private readonly ITextToSpeechService _textToSpeechService;

        public FlashCardsController(IWordsApiService wordsApiService, ITextToSpeechService textToSpeechService)
        {
            _wordsApiService = wordsApiService;
            _textToSpeechService = textToSpeechService;
        }

        [HttpGet("{word}")]
        public async Task<IActionResult> GetFullDefinitionWord(string word)
        {
            var apiResponse = await _wordsApiService.GetWordFullInfoAsync(word);

            if (apiResponse.GetResponse().StatusCode == StatusCodes.Status200OK)
            {
                var resultData = JsonConvert.DeserializeObject<Root>(apiResponse.GetResponse().Data);
                var textToSpeechParams = new TextToSpeechRequestDto
                {
                    Title = word,
                    Text = word,
                    LanguageCode = "en-US",
                    AudioGender = "m",
                };
                var audioResponse = await _textToSpeechService.ConvertTextToSpeech(textToSpeechParams);
                resultData.Pronunciation.AudioURL = audioResponse.GetResponse().Data;
                apiResponse.SetResponse(true, StatusCodes.Status200OK, "", JsonConvert.SerializeObject(resultData));
            }

            await apiResponse.ToHttpResponse(this.HttpContext);

            return new EmptyResult();
        }

        [HttpGet("{word}")]
        public async Task<IActionResult> GetWordPronunciation(string word)
        {
            var apiResponse = await _wordsApiService.GetWordPronunciationCodeAsync(word);
            var resultData = JsonConvert.DeserializeObject<Root>(apiResponse.GetResponse().Data);

            var textToSpeechParams = new TextToSpeechRequestDto
            {
                Title = word,
                Text = word,
                LanguageCode = "en-US",
                AudioGender = "m",
            };
            var audioResponse = await _textToSpeechService.ConvertTextToSpeech(textToSpeechParams);
            resultData.Pronunciation.AudioURL = audioResponse.GetResponse().Data;
            apiResponse.SetResponse(true, StatusCodes.Status200OK, "", JsonConvert.SerializeObject(resultData));

            await apiResponse.ToHttpResponse(this.HttpContext);

            return new EmptyResult();
        }
    }
}
