using System.ComponentModel.DataAnnotations;

namespace Wordpicker_API.Models.WordAPI
{
    public class WordAPIExample
    {
        [Key]
        public int ExampleId { get; set; }

        [Required]
        public string Example { get; set; } = string.Empty;

        public int WordId { get; set; }

        public virtual WordAPIWord Word { get; set; }
    }
}
