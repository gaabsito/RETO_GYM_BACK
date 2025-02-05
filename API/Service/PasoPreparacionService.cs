using System.Collections.Generic;
using System.Threading.Tasks;
using RecetasAPI.Models;
using RecetasAPI.Repositories;

namespace RecetasAPI.Services
{
    public interface IPasoPreparacionService
    {
        Task<IEnumerable<PasoPreparacion>> GetAllAsync();
        Task<PasoPreparacion> GetByIdAsync(int id);
        Task AddAsync(PasoPreparacion pasoPreparacion);
        Task UpdateAsync(PasoPreparacion pasoPreparacion);
        Task DeleteAsync(int id);
    }

    public class PasoPreparacionService : IPasoPreparacionService
    {
        private readonly IPasoPreparacionRepository _pasoPreparacionRepository;

        public PasoPreparacionService(IPasoPreparacionRepository pasoPreparacionRepository)
        {
            _pasoPreparacionRepository = pasoPreparacionRepository;
        }

        public async Task<IEnumerable<PasoPreparacion>> GetAllAsync()
        {
            return await _pasoPreparacionRepository.GetAllAsync();
        }

        public async Task<PasoPreparacion> GetByIdAsync(int id)
        {
            return await _pasoPreparacionRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(PasoPreparacion pasoPreparacion)
        {
            await _pasoPreparacionRepository.AddAsync(pasoPreparacion);
        }

        public async Task UpdateAsync(PasoPreparacion pasoPreparacion)
        {
            await _pasoPreparacionRepository.UpdateAsync(pasoPreparacion);
        }

        public async Task DeleteAsync(int id)
        {
            await _pasoPreparacionRepository.DeleteAsync(id);
        }
    }
}
