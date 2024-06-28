using Microsoft.EntityFrameworkCore;
using Wordpicker_API.Models;
using Wordpicker_API.Models.WordAPI;

namespace Wordpicker_API.Contexts
{
    public class WordpickerContext : DbContext
    {
        public WordpickerContext(DbContextOptions<WordpickerContext> options) : base(options) { }

        public DbSet<RegisteredWords> RegisteredWords { get; set; }
        public DbSet<WordAPIWord> WordAPIWord { get; set; }
        public DbSet<WordAPIType> WordAPIType { get; set; }
        public DbSet<WordAPIExample> WordAPIExample { get; set; }
        public DbSet<WordAPIDefinition> WordAPIDefinition { get; set; }
        public DbSet<WordAPISynonym> WordAPISynonym { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationships
            modelBuilder.Entity<WordAPIType>()
                .HasOne(t => t.Word)
                .WithMany(w => w.Types)
                .HasForeignKey(t => t.WordId);

            modelBuilder.Entity<WordAPIExample>()
                .HasOne(e => e.Word)
                .WithMany(w => w.Examples)
                .HasForeignKey(e => e.WordId);

            modelBuilder.Entity<WordAPIDefinition>()
                .HasOne(d => d.Word)
                .WithMany(w => w.Definitions)
                .HasForeignKey(d => d.WordId);

            modelBuilder.Entity<WordAPISynonym>()
                .HasOne(s => s.Word)
                .WithMany(w => w.Synonyms)
                .HasForeignKey(s => s.WordId);
        }
    }
}
