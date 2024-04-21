using System.Text.Json.Serialization;

namespace Wordpicker_API.DTOs
{
    public class Result
    {
        [JsonPropertyName("definition")]
        public string? Definition { get; set; }

        [JsonPropertyName("partOfSpeech")]
        public string? PartOfSpeech { get; set; }

        [JsonPropertyName("synonyms")]
        public List<string>? Synonyms { get; set; }

        [JsonPropertyName("typeOf")]
        public List<string>? TypeOf { get; set; }

        [JsonPropertyName("hasTypes")]
        public List<string>? HasTypes { get; set; }

        [JsonPropertyName("examples")]
        public List<string>? Examples { get; set; }
    }

    public class Syllables
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("list")]
        public List<string>? List { get; set; }
    }

    public class Pronunciation
    {
        [JsonPropertyName("all")]
        public string? All { get; set; }

        [JsonPropertyName("audio")]
        public string? AudioURL { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("word")]
        public string? Word { get; set; }

        [JsonPropertyName("results")]
        public List<Result>? Results { get; set; }

        [JsonPropertyName("syllables")]
        public Syllables? Syllables { get; set; }

        [JsonPropertyName("pronunciation")]
        public Pronunciation? Pronunciation { get; set; }

        [JsonPropertyName("frequency")]
        public double Frequency { get; set; }

        [JsonPropertyName("definitions_jp")]
        public List<string>? DefinitionsJp { get; set; }
    }
}
