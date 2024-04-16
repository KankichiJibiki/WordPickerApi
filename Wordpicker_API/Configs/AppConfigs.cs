namespace Wordpicker_API.Configs
{
    public class AppConfigs : IAppConfigs
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

        public string GetDeepLApiKey()
        {
            return _configuration["DeepLApiKey"];
        }
        public string GetAWSRegion()
        {
            return _configuration["AWS:Region"];
        }
        public string GetAWSProfileName()
        {
            return _configuration["AWS:ProfileName"];
        }
        public string GetS3BucketName()
        {
            return _configuration["AWS:BucketName"];
        }
        public string GetS3AccessKey()
        {
            return _configuration["AWS:AccessKey"];
        }
        public string GetS3SecretKey()
        {
            return _configuration["AWS:SecretKey"];
        }
    }
}
