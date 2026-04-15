using BCP.Models;
using Microsoft.EntityFrameworkCore;

namespace BCP.Data
{
    /// <summary>
    /// A classe BookContext é responsável por representar o contexto da base de dados para a aplicação.
    /// Ela herda da classe DbContext do Entity Framework Core.
    /// O BookContext é configurado para usar as opções fornecidas no construtor,
    /// permitindo a conexão com a base de dados e a definição das entidades que serão mapeadas para as tabelas.
    /// A propriedade Books representa a coleção de livros e permite realizar operações de CRUD sobre os registos dos livros.
    /// </summary>
    /// <param name="option">
    /// As opções de configuração para o contexto, que incluem detalhes como a string de conexão e outras configurações relacionadas à base de dados.
    /// </param>
    public class BookContext(DbContextOptions<BookContext> option) : DbContext(option)
    {
        public DbSet<Book> Books { get; set; }
    }
}
