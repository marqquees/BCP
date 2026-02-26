using BCP.Models;
using Microsoft.EntityFrameworkCore;

namespace BCP.Data
{
    public class BookContext(DbContextOptions<BookContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ISBN)
                    .HasMaxLength(13);

                entity.Property(e => e.EAN)
                    .HasMaxLength(13);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Subject)
                    .HasMaxLength(100);

                entity.Property(e => e.Subtitle)
                    .HasMaxLength(255);

                entity.Property(e => e.Edition)
                    .HasMaxLength(50);

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Gender)
                    .HasMaxLength(100);

                entity.Property(e => e.Publisher)
                    .HasMaxLength(255);

                entity.Property(e => e.PublicationDate)
                    .HasColumnType("date");

                entity.Property(e => e.Language)
                    .HasMaxLength(50);

                entity.Property(e => e.Format)
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .HasColumnType("text");

                entity.Property(e => e.Note)
                    .HasColumnType("text");
            });
        }
    }
}
