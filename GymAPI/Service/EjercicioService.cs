using GymAPI.Models;
using GymAPI.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymAPI.Services
{
    public class EjercicioService : IEjercicioService
    {
        private readonly IEjercicioRepository _repository;

        public EjercicioService(IEjercicioRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Ejercicio>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Ejercicio?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Ejercicio ejercicio)
        {
            await _repository.AddAsync(ejercicio);
        }

        public async Task UpdateAsync(Ejercicio ejercicio)
        {
            await _repository.UpdateAsync(ejercicio);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
