// GymAPI/Repository/ILogroRepository.cs
using GymAPI.Models;

namespace GymAPI.Repositories
{
    public interface ILogroRepository
    {
        Task<List<Logro>> GetAllAsync();
        Task<Logro?> GetByIdAsync(int id);
        Task<List<Logro>> GetByCategoriaAsync(string categoria);
        Task<int> AddAsync(Logro logro);
        Task UpdateAsync(Logro logro);
        Task DeleteAsync(int id);
    }
}