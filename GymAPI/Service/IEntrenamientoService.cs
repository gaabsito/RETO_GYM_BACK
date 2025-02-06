using GymAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymAPI.Services
{
    public interface IEntrenamientoService
    {
        Task<List<Entrenamiento>> GetAllAsync();
        Task<Entrenamiento?> GetByIdAsync(int id);
        Task AddAsync(Entrenamiento entrenamiento);
        Task UpdateAsync(Entrenamiento entrenamiento);
        Task DeleteAsync(int id);
    }
}