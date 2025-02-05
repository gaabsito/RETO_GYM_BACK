using System.Collections.Generic;
using System.Threading.Tasks;
using RecetasAPI.Models;
using RecetasAPI.Repositories;

namespace RecetasAPI.Services
{
    public interface IRecetaEtiquetasService
    {
        Task<IEnumerable<RecetaEtiquetas>> GetAllAsync();
        Task<RecetaEtiquetas> GetByIdAsync(int recetaId, int etiquetaId);
        Task AddAsync(RecetaEtiquetas relacion);
        Task DeleteAsync(int recetaId, int etiquetaId);
    }

    public class RecetaEtiquetasService : IRecetaEtiquetasService
    {
        private readonly IRecetaEtiquetasRepository _recetaEtiquetasRepository;

        public RecetaEtiquetasService(IRecetaEtiquetasRepository recetaEtiquetasRepository)
        {
            _recetaEtiquetasRepository = recetaEtiquetasRepository;
        }

        public async Task<IEnumerable<RecetaEtiquetas>> GetAllAsync()
        {
            return await _recetaEtiquetasRepository.GetAllAsync();
        }

        public async Task<RecetaEtiquetas> GetByIdAsync(int recetaId, int etiquetaId)
        {
            return await _recetaEtiquetasRepository.GetByIdAsync(recetaId, etiquetaId);
        }

        public async Task AddAsync(RecetaEtiquetas relacion)
        {
            await _recetaEtiquetasRepository.AddAsync(relacion);
        }

        public async Task DeleteAsync(int recetaId, int etiquetaId)
        {
            await _recetaEtiquetasRepository.DeleteAsync(recetaId, etiquetaId);
        }
    }
}
