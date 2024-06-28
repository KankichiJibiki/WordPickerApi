using System.ComponentModel.DataAnnotations;

namespace Wordpicker_API.Models.WordAPI
{
    public class WordAPIDefinition
    {
        [Key]
        public int DefinitionId { get; set; }

        public string DefinitionJP { get; set; } = string.Empty;
        public string DefinitionEN { get; set; } = string.Empty;

        public int WordId { get; set; }

        public virtual WordAPIWord Word { get; set; }
    }
}
