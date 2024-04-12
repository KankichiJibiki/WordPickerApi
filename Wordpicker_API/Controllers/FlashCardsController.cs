using Microsoft.AspNetCore.Mvc;
using Wordpicker_API.DTOs;
using Wordpicker_API.Services.DeepLService;
using Wordpicker_API.Services.WordsApiService;
using Wordpicker_API.Utils;

namespace Wordpicker_API.Controllers
{
    [ApiController]
    [Route("wordpickerapi/[controller]/[action]")]
    public class FlashCardsController : Controller
    {
        private readonly IWordsApiService _wordsApiService;
        private readonly IDeepLService _deepLService;

        public FlashCardsController(IWordsApiService wordsApiService, IDeepLService deepLService)
        {
            _wordsApiService = wordsApiService;
            _deepLService = deepLService;
        }

        [HttpGet("{word}")]
        public async Task<IActionResult> GetDefinitionWord(string word)
        {
            var apiResponse = await _wordsApiService.GetWordAsync(word);
            await apiResponse.ToHttpResponse(this.HttpContext);

            return new EmptyResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetTextsTranslated(TranslateRequestDto[] request)
        {
            var deepLResponse = await _deepLService.GetTextsTranslated(request);
            await deepLResponse.ToHttpResponse(this.HttpContext);

            return new EmptyResult();
        }
    }
}
