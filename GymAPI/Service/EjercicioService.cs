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
            if (string.IsNullOrWhiteSpace(ejercicio.Nombre))
            {
                throw new ArgumentException("El nombre del ejercicio no puede estar vacío.");
            }

            await _repository.AddAsync(ejercicio);
        }

        public async Task UpdateAsync(Ejercicio ejercicio)
        {
            var existingEjercicio = await _repository.GetByIdAsync(ejercicio.EjercicioID);
            if (existingEjercicio == null)
            {
                throw new KeyNotFoundException($"No se encontró el ejercicio con ID {ejercicio.EjercicioID}");
            }

            await _repository.UpdateAsync(ejercicio);
        }

        public async Task DeleteAsync(int id)
        {
            var existingEjercicio = await _repository.GetByIdAsync(id);
            if (existingEjercicio == null)
            {
                throw new KeyNotFoundException($"No se encontró el ejercicio con ID {id}");
            }

            await _repository.DeleteAsync(id);
        }
    }
}
