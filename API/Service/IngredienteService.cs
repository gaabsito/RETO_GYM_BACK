using System.Collections.Generic;
using System.Threading.Tasks;
using RecetasAPI.Models;
using RecetasAPI.Repositories;

namespace RecetasAPI.Services
{
    public interface IIngredienteService
    {
        Task<IEnumerable<Ingrediente>> GetAllAsync();
        Task<Ingrediente> GetByIdAsync(int id);
        Task AddAsync(Ingrediente ingrediente);
        Task UpdateAsync(Ingrediente ingrediente);
        Task DeleteAsync(int id);
    }

    public class IngredienteService : IIngredienteService
    {
        private readonly IIngredienteRepository _ingredienteRepository;

        public IngredienteService(IIngredienteRepository ingredienteRepository)
        {
            _ingredienteRepository = ingredienteRepository;
        }

        public async Task<IEnumerable<Ingrediente>> GetAllAsync()
        {
            return await _ingredienteRepository.GetAllAsync();
        }

        public async Task<Ingrediente> GetByIdAsync(int id)
        {
            return await _ingredienteRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Ingrediente ingrediente)
        {
            await _ingredienteRepository.AddAsync(ingrediente);
        }

        public async Task UpdateAsync(Ingrediente ingrediente)
        {
            await _ingredienteRepository.UpdateAsync(ingrediente);
        }

        public async Task DeleteAsync(int id)
        {
            await _ingredienteRepository.DeleteAsync(id);
        }
    }
}
