using BibliotecaCarvalhoPereira.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaCarvalhoPereira.Data
{
    public class LivroContext: DbContext
    {
        public LivroContext(DbContextOptions<LivroContext> opcoes) 
            : base(opcoes) { }
         
        public DbSet<Livro> Livros { get; set; }
    }
}
