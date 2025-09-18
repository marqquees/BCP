using BibliotecaCarvalhoPereira.Data;
using BibliotecaCarvalhoPereira.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data.Common;

namespace BibliotecaCarvalhoPereira.Services
{
    public class BookOperation(BookContext context, ILogger<BookOperation> logger)
    {
        private readonly BookContext _context = context;
        private readonly ILogger<BookOperation> _logger = logger;

        public async Task<Book> AddBookAsync(Book book)
        {
            try
            {
                EntityEntry<Book> b = await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();

                return b.Entity;
            }
            catch (DbException error)
            {
                _logger.LogError(error, "Erro ao adicionar o book {TitleBook}.", book.Title);
                return book;
            }
        }

        public async Task<List<Book>> ListBookAsync()
        {
            try
            {
                return await _context.Books.ToListAsync();
            }
            catch (DbException error)
            {
                _logger.LogError(error, "Erro ao carregar a lista dos livros.");
                return [];
            }
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            try
            {
                // Verificar se a entidade já está sendo rastreada.
                EntityEntry? trackedEntity = _context.ChangeTracker.Entries<Book>()
                    .FirstOrDefault(e => e.Entity.Id == book.Id);

                // Se a entidade já está sendo rastreada, atualiza os valores.
                if (trackedEntity != null)
                    _context.Entry(trackedEntity.Entity).CurrentValues.SetValues(book);
                else
                    _context.Books.Update(book);

                await _context.SaveChangesAsync();
                return book;
            }
            catch (DbException error)
            {
                _logger.LogError(error, "Erro ao atualizar o book {TitleBook}.", book.Title);
                return book;
            }
        }

        public async Task<Book?> FindBookByIdAsync(int id)
        {
            try
            {
                return await _context.Books.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
            }
            catch (DbException error)
            {
                _logger.LogError(error, "Erro ao buscar o book com ID {IdBook}.", id);
                return null;
            }
        }

        public async Task<bool> RemoveBookAsync(int id)
        {
            try
            {
                // Verifica se o book existe antes de tentar removê-lo.
                Book? book = await _context.Books.FindAsync(id);

                if (book == null)
                    return false;

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbException error)
            {
                _logger.LogError(error, "Erro ao remover o livro com o ID {IdBook}.", id);
                return false;
            }
        }
    }
}
