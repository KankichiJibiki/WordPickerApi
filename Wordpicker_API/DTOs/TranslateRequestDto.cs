namespace Wordpicker_API.DTOs
{
    public class TranslateRequestDto
    {
        public string Text { get; set; } = string.Empty;
        public bool EnToJp { get; set; }
    }
}
