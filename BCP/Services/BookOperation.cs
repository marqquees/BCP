using BCP.Data;
using BCP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BCP.Services
{
    public class BookOperation(BookContext context, ILogger<BookOperation> logger)
    {
        private readonly BookContext _context = context;
        private readonly ILogger<BookOperation> _logger = logger;

        /// <summary>
        /// Recupera a lista de livros do catálogo.
        /// </summary>
        /// <returns>
        /// Uma lista de livros do catálogo ou uma lista vazia se ocorrer um erro durante a recuperação dos dados.
        /// </returns>
        public async Task<List<Book>> ListBookAsync()
        {
            try
            {
                return await _context.Books.ToListAsync();
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Erro ao carregar a lista do catálogo dos livros.");
                throw;
            }
        }
        
        /// <summary>
        /// Adiciona um novo livro ao catálogo.
        /// Se o livro já tiver um ISBN definido,
        /// verifica se já existe um livro com o mesmo ISBN na base de dados.
        /// Se existir, lança uma exceção informando que o livro já existe e não o adiciona.
        /// </summary>
        /// <param name="book">
        /// O livro a ser adicionado ao catálogo.
        /// </param>
        /// <returns>
        /// O livro adicionado ao catálogo ou o livro original se ocorrer um erro durante a adição dos dados.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Lançada quando um livro com o mesmo ISBN já existe no catálogo.
        /// </exception>
        public async Task<Book> AddBookAsync(Book book)
        {
            try
            {
                if (!string.IsNullOrEmpty(book.ISBN))
                {
                    bool isbnExists = await _context.Books.AnyAsync(b => b.ISBN == book.ISBN);
                    if (isbnExists)
                        throw new InvalidOperationException($"O livro com ISBN {book.ISBN} já existe no catálogo.");
                }
                
                // Se a edição do livro for nula, atribui o valor 1.
                book.Edition ??= 1;
                
                EntityEntry<Book> b = await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();
                return b.Entity;
            }
            catch (DbUpdateException error)
            {
                _logger.LogError(error, "Erro ao adicionar o livro {TitleBook}.", book.Title);
                throw;
            }
        }

        /// <summary>
        /// Atualiza as informações de um livro existente no catálogo.
        /// </summary>
        /// <param name="book">
        /// O livro com as informações atualizadas.
        /// O livro deve conter um ID válido para que a atualização seja realizada corretamente.
        /// </param>
        /// <returns>
        /// O livro atualizado no catálogo ou o livro original se ocorrer um erro durante a atualização dos dados.
        /// </returns> 
        public async Task<Book> UpdateDataBookAsync(Book book)
        {
            try
            {
                // Verificar se a entidade já está sendo rastreada.
                EntityEntry? trackedBook = _context.ChangeTracker.Entries<Book>()
                    .FirstOrDefault(b => b.Entity.Id == book.Id);
                
                // Se a entidade já está sendo rastreada, atualiza os valores.
                if (trackedBook != null)
                    _context.Entry(trackedBook.Entity).CurrentValues.SetValues(book);
                else
                    _context.Books.Update(book);

                await _context.SaveChangesAsync();
                return book;
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Erro ao atualizar o livro {TitleBook}.", book.Title);
                throw;
            }
        }

        /// <summary>
        /// Busca um livro no catálogo pelo seu ID.
        /// </summary>
        /// <param name="idBook">
        /// O ID do livro a ser buscado no catálogo.
        /// </param>
        /// <returns>
        /// O livro encontrado no catálogo ou null se o livro não for encontrado
        /// ou ocorrer um erro durante a busca dos dados.
        /// </returns>
        public async Task<Book?> FindBookByIdAsync(int idBook)
        {
            try
            {
                return await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == idBook);
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Erro ao buscar o livro com ID: {IdBook}.", idBook);
                throw;
            }
        }

        /// <summary>
        /// Remove um livro do catálogo pelo seu ID.
        /// </summary>
        /// <param name="idBook">
        /// O ID do livro a ser removido do catálogo.
        /// </param>
        /// <returns>
        /// true se o livro foi removido com sucesso do catálogo e false se o livro não for encontrado
        /// ou ocorrer um erro durante a remoção dos dados.
        /// </returns>
        public async Task<bool> RemoveBookAsync(int idBook)
        {
            try
            {
                // Verifica se o livro existe antes de tentar removê-lo.
                Book? book = await _context.Books.FindAsync(idBook);
                if (book is null) return false;
                
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Erro ao remover o livro com o ID: {IdBook}.", idBook);
                throw;
            }
        }
    }
}
