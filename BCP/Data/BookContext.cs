using BCP.Models;
using Microsoft.EntityFrameworkCore;

namespace BCP.Data
{
    public class BookContext(DbContextOptions<BookContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// Configura o modelo de dados para a entidade "Book" usando o Fluent API do Entity Framework Core.
        /// </summary>
        /// <param name="modelBuilder">
        /// O objeto ModelBuilder usado para configurar o modelo de dados.
        /// Ele permite definir as propriedades, chaves primárias, restrições e outras configurações para a entidade "Book".
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
                
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(150);
                
                entity.Property(e => e.ISBN)
                    .HasMaxLength(13);

                entity.Property(e => e.Format)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.Subject)
                    .HasMaxLength(100);
                
                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(40);
                
                entity.Property(e => e.Publisher)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.Edition)
                    .HasMaxLength(4);

                entity.Property(e => e.PublicationYear)
                    .IsRequired()
                    .HasMaxLength(4);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.Note)
                    .HasMaxLength(200);

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(40);
            });
        }
    }
}
