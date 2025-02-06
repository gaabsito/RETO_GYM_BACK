using GymAPI.Models;
using GymAPI.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymAPI.Services
{
    public class EntrenamientoService : IEntrenamientoService
    {
        private readonly IEntrenamientoRepository _repository;

        public EntrenamientoService(IEntrenamientoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Entrenamiento>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Entrenamiento?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Entrenamiento entrenamiento)
        {
            await _repository.AddAsync(entrenamiento);
        }

        public async Task UpdateAsync(Entrenamiento entrenamiento)
        {
            await _repository.UpdateAsync(entrenamiento);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
