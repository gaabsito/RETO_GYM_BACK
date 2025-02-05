using System.Collections.Generic;
using System.Threading.Tasks;
using RecetasAPI.Models;
using RecetasAPI.Repositories;

namespace RecetasAPI.Services
{
    public interface IEtiquetaService
    {
        Task<IEnumerable<Etiqueta>> GetAllAsync();
        Task<Etiqueta> GetByIdAsync(int id);
        Task AddAsync(Etiqueta etiqueta);
        Task UpdateAsync(Etiqueta etiqueta);
        Task DeleteAsync(int id);
    }

    public class EtiquetaService : IEtiquetaService
    {
        private readonly IEtiquetaRepository _etiquetaRepository;

        public EtiquetaService(IEtiquetaRepository etiquetaRepository)
        {
            _etiquetaRepository = etiquetaRepository;
        }

        public async Task<IEnumerable<Etiqueta>> GetAllAsync()
        {
            return await _etiquetaRepository.GetAllAsync();
        }

        public async Task<Etiqueta> GetByIdAsync(int id)
        {
            return await _etiquetaRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Etiqueta etiqueta)
        {
            await _etiquetaRepository.AddAsync(etiqueta);
        }

        public async Task UpdateAsync(Etiqueta etiqueta)
        {
            await _etiquetaRepository.UpdateAsync(etiqueta);
        }

        public async Task DeleteAsync(int id)
        {
            await _etiquetaRepository.DeleteAsync(id);
        }
    }
}
