using BCP.Models;
using Microsoft.EntityFrameworkCore;

namespace BCP.Data
{
    public class BookContext(DbContextOptions<BookContext> opcoes) : DbContext(opcoes)
    {
        public DbSet<Book> Books { get; set; }
    }
}
