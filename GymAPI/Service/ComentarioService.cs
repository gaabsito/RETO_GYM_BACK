using GymAPI.Models;
using GymAPI.Repositories;

namespace GymAPI.Services
{
    public class ComentarioService : IComentarioService
    {
        private readonly IComentarioRepository _repository;

        public ComentarioService(IComentarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Comentario>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Comentario?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<Comentario>> GetByEntrenamientoIdAsync(int entrenamientoId)
        {
            return await _repository.GetByEntrenamientoIdAsync(entrenamientoId);
        }

        public async Task AddAsync(Comentario comentario)
        {
            await _repository.AddAsync(comentario);
        }

        public async Task UpdateAsync(Comentario comentario)
        {
            await _repository.UpdateAsync(comentario);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}