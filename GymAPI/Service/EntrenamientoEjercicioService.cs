using GymAPI.Models;
using GymAPI.Repositories;

namespace GymAPI.Services
{
    public class EntrenamientoEjercicioService : IEntrenamientoEjercicioService
    {
        private readonly IEntrenamientoEjercicioRepository _repository;

        public EntrenamientoEjercicioService(IEntrenamientoEjercicioRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<EntrenamientoEjercicio>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<EntrenamientoEjercicio>> GetByEntrenamientoAsync(int entrenamientoID)
        {
            return await _repository.GetByEntrenamientoAsync(entrenamientoID);
        }

        public async Task<EntrenamientoEjercicio?> GetByIdAsync(int entrenamientoID, int ejercicioID)
        {
            return await _repository.GetByIdAsync(entrenamientoID, ejercicioID);
        }

        public async Task AddAsync(EntrenamientoEjercicio entrenamientoEjercicio)
        {
            await _repository.AddAsync(entrenamientoEjercicio);
        }

        public async Task UpdateAsync(int entrenamientoID, int ejercicioID, EntrenamientoEjercicio entrenamientoEjercicio)
        {
            await _repository.UpdateAsync(entrenamientoID, ejercicioID, entrenamientoEjercicio);
        }

        public async Task RemoveAsync(int entrenamientoID, int ejercicioID)
        {
            await _repository.RemoveAsync(entrenamientoID, ejercicioID);
        }
    }
}
