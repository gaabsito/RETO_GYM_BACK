using System.Collections.Generic;
using System.Threading.Tasks;
using RecetasAPI.Models;
using RecetasAPI.Repositories;

namespace RecetasAPI.Services
{
    public interface IComentarioService
    {
        Task<IEnumerable<Comentario>> GetAllAsync();
        Task<Comentario> GetByIdAsync(int id);
        Task AddAsync(Comentario comentario);
        Task UpdateAsync(Comentario comentario);
        Task DeleteAsync(int id);
    }

    public class ComentarioService : IComentarioService
    {
        private readonly IComentarioRepository _comentarioRepository;

        public ComentarioService(IComentarioRepository comentarioRepository)
        {
            _comentarioRepository = comentarioRepository;
        }

        public async Task<IEnumerable<Comentario>> GetAllAsync()
        {
            return await _comentarioRepository.GetAllAsync();
        }

        public async Task<Comentario> GetByIdAsync(int id)
        {
            return await _comentarioRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Comentario comentario)
        {
            await _comentarioRepository.AddAsync(comentario);
        }

        public async Task UpdateAsync(Comentario comentario)
        {
            await _comentarioRepository.UpdateAsync(comentario);
        }

        public async Task DeleteAsync(int id)
        {
            await _comentarioRepository.DeleteAsync(id);
        }
    }
}
