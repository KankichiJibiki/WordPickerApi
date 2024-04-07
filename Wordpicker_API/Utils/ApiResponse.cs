using Newtonsoft.Json;

namespace Wordpicker_API.Utils
{
    public class Response
    {
        public bool Success { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public string Data { get; set; } = string.Empty;
    }
    public class ApiResponse
    {
        private readonly Response _response;

        public ApiResponse()
        {
            _response = new Response();
        }
        public void SetResponse(bool success, int statusCode, string message, string data)
        {
            _response.Success = success;
            _response.StatusCode = statusCode;
            _response.Message = message;
            _response.Data = data;
        }

        public Response GetResponse()
        {
            return _response;
        }

        public async Task<HttpContext> ToHttpResponse(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (_response == null)
            {
                throw new InvalidOperationException("Response object is null");
            }

            if (string.IsNullOrEmpty(_response.Data))
            {
                context.Response.StatusCode = StatusCodes.Status204NoContent;
                return context;
            }
            else
            {
                context.Response.StatusCode = _response.StatusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(_response.Data);
            }

            return context;
        }
    }
}
