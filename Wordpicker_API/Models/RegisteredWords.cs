using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Wordpicker_API.Models
{

    [Comment("Simple English word data table")]
    public class RegisteredWords
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public string Word { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string DefinitionEn { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string DefinitionJP { get; set; } = string.Empty;

    }
}
