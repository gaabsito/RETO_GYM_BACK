using RecetasAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecetasAPI.Repositories
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);
    }
}
