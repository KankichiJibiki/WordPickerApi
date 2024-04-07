namespace Wordpicker_API.Configs
{
    public class AppConfigs: IAppConfigs
    {
        private readonly IConfiguration _configuration;

        public AppConfigs(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetWordsApiKey()
        {
            return _configuration["WordsApiKeys"];
        }

        public string GetWordsApiHost()
        {
            return _configuration["WordsApiHost"];
        }

        public string GetWordsApiEndpoint()
        {
            return _configuration["WordsApiEndpoint"];
        }
    }
}
