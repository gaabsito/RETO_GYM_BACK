using GymAPI.Models;
using GymAPI.Repositories;

namespace GymAPI.Services
{
    public class RutinaCompletadaService : IRutinaCompletadaService
    {
        private readonly IRutinaCompletadaRepository _repository;

        public RutinaCompletadaService(IRutinaCompletadaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RutinaCompletada>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<RutinaCompletada?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<RutinaCompletada>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _repository.GetByUsuarioIdAsync(usuarioId);
        }

        public async Task<List<RutinaCompletada>> GetByEntrenamientoIdAsync(int entrenamientoId)
        {
            return await _repository.GetByEntrenamientoIdAsync(entrenamientoId);
        }

        public async Task<List<RutinaCompletada>> GetByUsuarioIdAndEntrenamientoIdAsync(int usuarioId, int entrenamientoId)
        {
            return await _repository.GetByUsuarioIdAndEntrenamientoIdAsync(usuarioId, entrenamientoId);
        }

        public async Task<int> AddAsync(RutinaCompletada rutinaCompletada)
        {
            return await _repository.AddAsync(rutinaCompletada);
        }

        public async Task UpdateAsync(RutinaCompletada rutinaCompletada)
        {
            await _repository.UpdateAsync(rutinaCompletada);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<int> GetCountLastWeekAsync(int usuarioId)
        {
            return await _repository.GetCountLastWeekAsync(usuarioId);
        }

        public async Task<int> GetCountLastMonthAsync(int usuarioId)
        {
            return await _repository.GetCountLastMonthAsync(usuarioId);
        }

        public async Task<int> GetTotalCountAsync(int usuarioId)
        {
            return await _repository.GetTotalCountAsync(usuarioId);
        }

        public async Task<(int EntrenamientoID, string Nombre, int Veces)> GetMostCompletedWorkoutAsync(int usuarioId)
        {
            return await _repository.GetMostCompletedWorkoutAsync(usuarioId);
        }
    }
}