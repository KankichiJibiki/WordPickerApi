namespace Wordpicker_API.Configs
{
    public interface IAppConfigs
    {
        string GetWordsApiKey();

        string GetWordsApiHost();

        string GetWordsApiEndpoint();
    }
}
