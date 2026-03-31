using BCP.Models;
using Microsoft.EntityFrameworkCore;

namespace BCP.Data
{
    public class BookContext(DbContextOptions<BookContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
    }
}
