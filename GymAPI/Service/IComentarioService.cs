using GymAPI.Models;

namespace GymAPI.Services
{
    public interface IComentarioService
    {
        Task<List<Comentario>> GetAllAsync();
        Task<Comentario?> GetByIdAsync(int id);
        Task<List<Comentario>> GetByEntrenamientoIdAsync(int entrenamientoId);
        Task AddAsync(Comentario comentario);
        Task UpdateAsync(Comentario comentario);
        Task DeleteAsync(int id);
    }
}