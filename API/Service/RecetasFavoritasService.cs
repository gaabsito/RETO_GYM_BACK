using System.Collections.Generic;
using System.Threading.Tasks;
using RecetasAPI.Models;
using RecetasAPI.Repositories;

namespace RecetasAPI.Services
{
    public interface IRecetasFavoritasService
    {
        Task<IEnumerable<RecetasFavoritas>> GetAllAsync();
        Task<RecetasFavoritas> GetByIdAsync(int usuarioId, int recetaId);
        Task AddAsync(RecetasFavoritas favorito);
        Task DeleteAsync(int usuarioId, int recetaId);
    }

    public class RecetasFavoritasService : IRecetasFavoritasService
    {
        private readonly IRecetasFavoritasRepository _recetasFavoritasRepository;

        public RecetasFavoritasService(IRecetasFavoritasRepository recetasFavoritasRepository)
        {
            _recetasFavoritasRepository = recetasFavoritasRepository;
        }

        public async Task<IEnumerable<RecetasFavoritas>> GetAllAsync()
        {
            return await _recetasFavoritasRepository.GetAllAsync();
        }

        public async Task<RecetasFavoritas> GetByIdAsync(int usuarioId, int recetaId)
        {
            return await _recetasFavoritasRepository.GetByIdAsync(usuarioId, recetaId);
        }

        public async Task AddAsync(RecetasFavoritas favorito)
        {
            await _recetasFavoritasRepository.AddAsync(favorito);
        }

        public async Task DeleteAsync(int usuarioId, int recetaId)
        {
            await _recetasFavoritasRepository.DeleteAsync(usuarioId, recetaId);
        }
    }
}
