// ===========================================
// 3. GymAPI/Service/UsuarioService.cs
// ===========================================
using GymAPI.Models;
using GymAPI.Repositories;

namespace GymAPI.Services
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

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _repository.GetByEmailAsync(email);
        }

        public async Task<int> AddAsync(Usuario usuario)
        {
            // Validación antes de agregar
            if (await _repository.EmailExistsAsync(usuario.Email))
            {
                throw new InvalidOperationException("Ya existe un usuario con este email");
            }
            
            return await _repository.AddAsync(usuario);
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            // Verificar que el usuario existe
            if (!await _repository.ExistsAsync(usuario.UsuarioID))
            {
                throw new InvalidOperationException("El usuario no existe");
            }
            
            // Verificar email único (excluyendo el usuario actual)
            if (await _repository.EmailExistsAsync(usuario.Email, usuario.UsuarioID))
            {
                throw new InvalidOperationException("Ya existe otro usuario con este email");
            }
            
            await _repository.UpdateAsync(usuario);
        }

        public async Task DeleteAsync(int id)
        {
            // Verificar que el usuario existe antes de intentar eliminar
            if (!await _repository.ExistsAsync(id))
            {
                throw new InvalidOperationException("El usuario no existe");
            }
            
            // Eliminar usuario (el repository maneja las foreign keys)
            await _repository.DeleteAsync(id);
        }

        public async Task UpdateResetTokenAsync(int userId, string? token, DateTime? expires)
        {
            await _repository.UpdateResetTokenAsync(userId, token, expires);
        }

        public async Task<Usuario?> GetByResetTokenAsync(string token)
        {
            return await _repository.GetByResetTokenAsync(token);
        }

        // Métodos adicionales para estadísticas
        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _repository.GetTotalUsersCountAsync();
        }

        public async Task<int> GetActiveUsersCountAsync()
        {
            return await _repository.GetActiveUsersCountAsync();
        }

        public async Task<int> GetAdminUsersCountAsync()
        {
            return await _repository.GetAdminUsersCountAsync();
        }

        public async Task<int> GetUsersRegisteredTodayAsync()
        {
            return await _repository.GetUsersRegisteredTodayAsync();
        }

        public async Task<int> GetUsersRegisteredThisMonthAsync()
        {
            return await _repository.GetUsersRegisteredThisMonthAsync();
        }
    }
}