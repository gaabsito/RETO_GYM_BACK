using System.Collections.Generic;
using System.Threading.Tasks;
using RecetasAPI.Models;
using RecetasAPI.Repositories;

namespace RecetasAPI.Services
{
    public interface IRecetaService
    {
        Task<IEnumerable<Receta>> GetAllAsync();
        Task<Receta> GetByIdAsync(int id);
        Task AddAsync(Receta receta);
        Task UpdateAsync(Receta receta);
        Task DeleteAsync(int id);
    }

    public class RecetaService : IRecetaService
    {
        private readonly IRecetaRepository _recetaRepository;

        public RecetaService(IRecetaRepository recetaRepository)
        {
            _recetaRepository = recetaRepository;
        }

        public async Task<IEnumerable<Receta>> GetAllAsync()
        {
            return await _recetaRepository.GetAllAsync();
        }

        public async Task<Receta> GetByIdAsync(int id)
        {
            return await _recetaRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Receta receta)
        {
            await _recetaRepository.AddAsync(receta);
        }

        public async Task UpdateAsync(Receta receta)
        {
            await _recetaRepository.UpdateAsync(receta);
        }

        public async Task DeleteAsync(int id)
        {
            await _recetaRepository.DeleteAsync(id);
        }
    }
}
