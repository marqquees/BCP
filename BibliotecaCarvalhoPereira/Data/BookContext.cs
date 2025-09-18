using BibliotecaCarvalhoPereira.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaCarvalhoPereira.Data
{
    public class BookContext(DbContextOptions<BookContext> opcoes) : DbContext(opcoes)
    {
        public DbSet<Book> Books { get; set; }
    }
}
