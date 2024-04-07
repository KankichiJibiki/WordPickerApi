using Microsoft.AspNetCore.Mvc;
using Wordpicker_API.Services.WordsApiService;
using Wordpicker_API.Utils;

namespace Wordpicker_API.Controllers
{
    [ApiController]
    [Route("wordpickerapi/[controller]/[action]")]
    public class FlashCardsController : Controller
    {
        private readonly IWordsApiService _wordsApiService;

        public FlashCardsController(IWordsApiService wordsApiService)
        {
            _wordsApiService = wordsApiService;
        }
        [HttpGet("{word}")]
        public async Task<IActionResult> GetDefinitionWord(string word)
        {
            var apiResponse = await _wordsApiService.GetWordAsync(word);
            await apiResponse.ToHttpResponse(this.HttpContext);

            return new EmptyResult();
        }
    }
}
