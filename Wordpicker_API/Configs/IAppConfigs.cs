namespace Wordpicker_API.Configs
{
    public interface IAppConfigs
    {
        string GetWordsApiKey();
        string GetWordsApiHost();
        string GetWordsApiEndpoint();
        string GetDeepLApiKey();
        string GetAWSRegion();
        string GetAWSProfileName();
        string GetS3BucketName();
        string GetS3AccessKey();
        string GetS3SecretKey();
        string GetTempAudioPrefix();
        string GetAudioContentType();
    }
}
