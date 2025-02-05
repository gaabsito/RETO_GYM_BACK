using RecetasAPI.Models;
using RecetasAPI.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecetasAPI.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Usuario usuario)
        {
            await _repository.AddAsync(usuario);
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            await _repository.UpdateAsync(usuario);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
