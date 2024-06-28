using System.ComponentModel.DataAnnotations;

namespace Wordpicker_API.Models.WordAPI
{
    public class WordAPIWord
    {
        [Key]
        public int WordId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Word { get; set; } = string.Empty;

        public string? Pronunciation { get; set; }

        public string? YourAudio { get; set; }

        public string? ExampleAudio { get; set; }

        public virtual ICollection<WordAPIType> Types { get; set; }
        public virtual ICollection<WordAPIExample> Examples { get; set; }
        public virtual ICollection<WordAPIDefinition> Definitions { get; set; }
        public virtual ICollection<WordAPISynonym> Synonyms { get; set; }

        public WordAPIWord()
        {
            Types = new HashSet<WordAPIType>();
            Examples = new HashSet<WordAPIExample>();
            Definitions = new HashSet<WordAPIDefinition>();
            Synonyms = new HashSet<WordAPISynonym>();
        }
    }
}
