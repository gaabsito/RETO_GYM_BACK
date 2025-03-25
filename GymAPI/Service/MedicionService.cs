using GymAPI.Models;
using GymAPI.Repositories;

namespace GymAPI.Services
{
    public class MedicionService : IMedicionService
    {
        private readonly IMedicionRepository _repository;

        public MedicionService(IMedicionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Medicion>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Medicion?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<Medicion>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _repository.GetByUsuarioIdAsync(usuarioId);
        }

        public async Task<List<MedicionResumen>> GetResumenByUsuarioIdAsync(int usuarioId)
        {
            return await _repository.GetResumenByUsuarioIdAsync(usuarioId);
        }

        public async Task<int> AddAsync(Medicion medicion)
        {
            return await _repository.AddAsync(medicion);
        }

        public async Task UpdateAsync(Medicion medicion)
        {
            await _repository.UpdateAsync(medicion);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}