namespace Wordpicker_API.DTOs
{
    public class TextToSpeechRequestDto
    {
        public string Text { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public string AudioGender { get; set; } = "m";
    }
}
