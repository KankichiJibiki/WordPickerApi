namespace Wordpicker_API.Repositories.WordAPI.WordAPIWord
{
    public interface IWordAPIWordRepository
    {
        Task<bool> RegisterWord(WordAPIWord word);
    }
}
