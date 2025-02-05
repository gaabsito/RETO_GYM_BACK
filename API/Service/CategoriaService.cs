using System.Collections.Generic;
using System.Threading.Tasks;
using RecetasAPI.Models;
using RecetasAPI.Repositories;

namespace RecetasAPI.Services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> GetAllAsync();
        Task<Categoria> GetByIdAsync(int id);
        Task AddAsync(Categoria categoria);
        Task UpdateAsync(Categoria categoria);
        Task DeleteAsync(int id);
    }

    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            return await _categoriaRepository.GetAllAsync();
        }

        public async Task<Categoria> GetByIdAsync(int id)
        {
            return await _categoriaRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Categoria categoria)
        {
            await _categoriaRepository.AddAsync(categoria);
        }

        public async Task UpdateAsync(Categoria categoria)
        {
            await _categoriaRepository.UpdateAsync(categoria);
        }

        public async Task DeleteAsync(int id)
        {
            await _categoriaRepository.DeleteAsync(id);
        }
    }
}
