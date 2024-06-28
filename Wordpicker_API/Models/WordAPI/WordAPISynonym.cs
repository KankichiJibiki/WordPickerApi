using System.ComponentModel.DataAnnotations;

namespace Wordpicker_API.Models.WordAPI
{
    public class WordAPISynonym
    {
        [Key]
        public int SynonymId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Synonym { get; set; } = string.Empty;

        public int WordId { get; set; }

        public virtual WordAPIWord Word { get; set; }
    }
}
