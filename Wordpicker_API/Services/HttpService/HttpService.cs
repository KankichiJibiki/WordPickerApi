using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.HttpService
{
    public class HttpService: IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiResponse _response;

        public HttpService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
            _response = new ApiResponse();
        }

        public async Task<ApiResponse> GetAsync(string url, IDictionary<string, string>? headers)
        {
            DefineHeaders(headers);

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                if(string.IsNullOrEmpty(content))
                {
                    _response.SetResponse(true, StatusCodes.Status204NoContent, "", content);
                    return _response;
                }
                _response.SetResponse(true, StatusCodes.Status200OK, "Success", content);

                return _response;
            } catch (Exception ex)
            {
                _response.SetResponse(true, StatusCodes.Status500InternalServerError, ex.Message, "");

                return _response;
            }
        }

        private void DefineHeaders(IDictionary<string, string>? headers)
        {
            if(headers != null)
            {
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        }
    }
}
