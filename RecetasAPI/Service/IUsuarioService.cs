using RecetasAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecetasAPI.Service
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);
    }
}
