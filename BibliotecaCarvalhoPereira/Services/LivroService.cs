using BibliotecaCarvalhoPereira.Data;
using BibliotecaCarvalhoPereira.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BibliotecaCarvalhoPereira.Services
{
    public class LivroService
    {
        private readonly LivroContext _contexto;
        public LivroService(LivroContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<Livro> AdicionarAsync(Livro livro)
        {
            EntityEntry<Livro> retorno = await _contexto.Livros.AddAsync(livro);
            await _contexto.SaveChangesAsync();

            return retorno.Entity;
        }

        public async Task<List<Livro>> ListarAsync()
        {
            return await _contexto.Livros.ToListAsync();
        }

        public async Task<Livro> AtualizarAsync(Livro livro)
        {
            // Verificar se a entidade já está sendo rastreada.
            EntityEntry? trackedEntity = _contexto.ChangeTracker.Entries<Livro>()
                .FirstOrDefault(e => e.Entity.Id == livro.Id);

            // Se a entidade já está sendo rastreada, atualiza os valores.
            if (trackedEntity != null)
                _contexto.Entry(trackedEntity.Entity).CurrentValues.SetValues(livro);
            else
                _contexto.Livros.Update(livro);

            await _contexto.SaveChangesAsync();
            
            return livro;
        }

        public async Task<Livro?> BuscarPorIdAsync(int id)
        {
            return await _contexto.Livros.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<bool> RemoverAsync(int id)
        {
            // Verifica se o livro existe antes de tentar removê-lo.
            Livro? livro = await _contexto.Livros.FindAsync(id);

            if (livro == null) return false; 

            _contexto.Livros.Remove(livro);
            await _contexto.SaveChangesAsync();

            return true;
        }
    }
}
