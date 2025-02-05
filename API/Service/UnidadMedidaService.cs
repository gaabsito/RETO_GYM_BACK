using System.Collections.Generic;
using System.Threading.Tasks;
using RecetasAPI.Models;
using RecetasAPI.Repositories;

namespace RecetasAPI.Services
{
    public interface IUnidadMedidaService
    {
        Task<IEnumerable<UnidadMedida>> GetAllAsync();
        Task<UnidadMedida> GetByIdAsync(int id);
        Task AddAsync(UnidadMedida unidadMedida);
        Task UpdateAsync(UnidadMedida unidadMedida);
        Task DeleteAsync(int id);
    }

    public class UnidadMedidaService : IUnidadMedidaService
    {
        private readonly IUnidadMedidaRepository _unidadMedidaRepository;

        public UnidadMedidaService(IUnidadMedidaRepository unidadMedidaRepository)
        {
            _unidadMedidaRepository = unidadMedidaRepository;
        }

        public async Task<IEnumerable<UnidadMedida>> GetAllAsync()
        {
            return await _unidadMedidaRepository.GetAllAsync();
        }

        public async Task<UnidadMedida> GetByIdAsync(int id)
        {
            return await _unidadMedidaRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(UnidadMedida unidadMedida)
        {
            await _unidadMedidaRepository.AddAsync(unidadMedida);
        }

        public async Task UpdateAsync(UnidadMedida unidadMedida)
        {
            await _unidadMedidaRepository.UpdateAsync(unidadMedida);
        }

        public async Task DeleteAsync(int id)
        {
            await _unidadMedidaRepository.DeleteAsync(id);
        }
    }
}
