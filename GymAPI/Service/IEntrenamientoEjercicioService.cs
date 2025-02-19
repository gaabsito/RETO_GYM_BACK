using GymAPI.Models;

namespace GymAPI.Services
{
    public interface IEntrenamientoEjercicioService
    {
        Task<List<EntrenamientoEjercicio>> GetAllAsync();
        Task<List<EntrenamientoEjercicio>> GetByEntrenamientoAsync(int entrenamientoID);
        Task<EntrenamientoEjercicio?> GetByIdAsync(int entrenamientoID, int ejercicioID);
        Task AddAsync(EntrenamientoEjercicio entrenamientoEjercicio);
        Task UpdateAsync(int entrenamientoID, int ejercicioID, EntrenamientoEjercicio entrenamientoEjercicio);
        Task RemoveAsync(int entrenamientoID, int ejercicioID);
    }
}
