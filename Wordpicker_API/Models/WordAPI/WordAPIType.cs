using System.ComponentModel.DataAnnotations;

namespace Wordpicker_API.Models.WordAPI
{
    public class WordAPIType
    {
        [Key]
        public int TypeId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Type { get; set; } = string.Empty;

        public int WordId { get; set; }

        public virtual WordAPIWord Word { get; set; }
    }
}
