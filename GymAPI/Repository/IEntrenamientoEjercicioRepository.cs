using GymAPI.Models;

namespace GymAPI.Repositories
{
    public interface IEntrenamientoEjercicioRepository
    {
        Task<List<EntrenamientoEjercicio>> GetAllAsync();
        Task<List<EntrenamientoEjercicio>> GetByEntrenamientoAsync(int entrenamientoID);
        Task AddAsync(EntrenamientoEjercicio entrenamientoEjercicio);
        Task UpdateAsync(int entrenamientoID, int ejercicioID, EntrenamientoEjercicio entrenamientoEjercicio);
        Task RemoveAsync(int entrenamientoID, int ejercicioID);
        Task<EntrenamientoEjercicio?> GetByIdAsync(int entrenamientoID, int ejercicioID);
    }
}
