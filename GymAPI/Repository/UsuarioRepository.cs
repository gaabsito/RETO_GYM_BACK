using Microsoft.Data.SqlClient;
using GymAPI.Models;
using Microsoft.Extensions.Logging;

namespace GymAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<UsuarioRepository>? _logger;

        public UsuarioRepository(string connectionString, ILogger<UsuarioRepository>? logger = null)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            var usuarios = new List<Usuario>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT UsuarioID, Nombre, Apellido, Email, FechaRegistro, 
                               EstaActivo, EsAdmin, Edad, Altura, Peso, FotoPerfilURL 
                               FROM Usuarios ORDER BY UsuarioID";
                
                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var usuario = new Usuario
                        {
                            UsuarioID = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            FechaRegistro = reader.GetDateTime(4),
                            EstaActivo = reader.GetBoolean(5),
                            EsAdmin = reader.GetBoolean(6),
                            Edad = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                            Altura = reader.IsDBNull(8) ? null : reader.GetFloat(8),
                            Peso = reader.IsDBNull(9) ? null : reader.GetFloat(9),
                            FotoPerfilURL = reader.IsDBNull(10) ? null : reader.GetString(10)
                        };
                        usuarios.Add(usuario);
                    }
                }
            }
            return usuarios;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            Usuario? usuario = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT UsuarioID, Nombre, Apellido, Email, Password, 
                               FechaRegistro, EstaActivo, EsAdmin, Edad, Altura, Peso, 
                               FotoPerfilURL, ResetPasswordToken, ResetPasswordExpires
                               FROM Usuarios WHERE UsuarioID = @Id";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            usuario = new Usuario
                            {
                                UsuarioID = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                Email = reader.GetString(3),
                                Password = reader.GetString(4),
                                FechaRegistro = reader.GetDateTime(5),
                                EstaActivo = reader.GetBoolean(6),
                                EsAdmin = reader.GetBoolean(7),
                                Edad = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                                Altura = reader.IsDBNull(9) ? null : reader.GetFloat(9),
                                Peso = reader.IsDBNull(10) ? null : reader.GetFloat(10),
                                FotoPerfilURL = reader.IsDBNull(11) ? null : reader.GetString(11),
                                ResetPasswordToken = reader.IsDBNull(12) ? null : reader.GetString(12),
                                ResetPasswordExpires = reader.IsDBNull(13) ? null : reader.GetDateTime(13)
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            Usuario? usuario = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT UsuarioID, Nombre, Apellido, Email, Password, 
                               FechaRegistro, EstaActivo, EsAdmin, ResetPasswordToken, ResetPasswordExpires,
                               Edad, Altura, Peso, FotoPerfilURL 
                               FROM Usuarios WHERE Email = @Email";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email.Trim().ToLower());
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            usuario = new Usuario
                            {
                                UsuarioID = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                Email = reader.GetString(3),
                                Password = reader.GetString(4),
                                FechaRegistro = reader.GetDateTime(5),
                                EstaActivo = reader.GetBoolean(6),
                                EsAdmin = reader.GetBoolean(7),
                                ResetPasswordToken = reader.IsDBNull(8) ? null : reader.GetString(8),
                                ResetPasswordExpires = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                                Edad = reader.IsDBNull(10) ? null : reader.GetInt32(10),
                                Altura = reader.IsDBNull(11) ? null : reader.GetFloat(11),
                                Peso = reader.IsDBNull(12) ? null : reader.GetFloat(12),
                                FotoPerfilURL = reader.IsDBNull(13) ? null : reader.GetString(13)
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        public async Task UpdateResetTokenAsync(int userId, string? token, DateTime? expires)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"UPDATE Usuarios 
                               SET ResetPasswordToken = @Token, 
                                   ResetPasswordExpires = @Expires 
                               WHERE UsuarioID = @UserId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Token", token as object ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Expires", expires as object ?? DBNull.Value);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Usuario?> GetByResetTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            Usuario? usuario = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT UsuarioID, Nombre, Apellido, Email, Password, 
                               FechaRegistro, EstaActivo, EsAdmin, ResetPasswordToken, 
                               ResetPasswordExpires, FotoPerfilURL, Edad, Altura, Peso
                               FROM Usuarios 
                               WHERE ResetPasswordToken = @Token 
                               AND ResetPasswordExpires > GETDATE()";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Token", token);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            usuario = new Usuario
                            {
                                UsuarioID = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                Email = reader.GetString(3),
                                Password = reader.GetString(4),
                                FechaRegistro = reader.GetDateTime(5),
                                EstaActivo = reader.GetBoolean(6),
                                EsAdmin = reader.GetBoolean(7),
                                ResetPasswordToken = reader.IsDBNull(8) ? null : reader.GetString(8),
                                ResetPasswordExpires = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                                FotoPerfilURL = reader.IsDBNull(10) ? null : reader.GetString(10),
                                Edad = reader.IsDBNull(11) ? null : reader.GetInt32(11),
                                Altura = reader.IsDBNull(12) ? null : reader.GetFloat(12),
                                Peso = reader.IsDBNull(13) ? null : reader.GetFloat(13)
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        public async Task<int> AddAsync(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Usuarios (Nombre, Apellido, Email, Password, EstaActivo, EsAdmin, 
                               Edad, Altura, Peso, FotoPerfilURL, FechaRegistro) 
                               VALUES (@Nombre, @Apellido, @Email, @Password, @EstaActivo, @EsAdmin, 
                               @Edad, @Altura, @Peso, @FotoPerfilURL, @FechaRegistro);
                               SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre ?? string.Empty);
                    command.Parameters.AddWithValue("@Apellido", usuario.Apellido ?? string.Empty);
                    command.Parameters.AddWithValue("@Email", usuario.Email?.Trim().ToLower() ?? string.Empty);
                    command.Parameters.AddWithValue("@Password", usuario.Password ?? string.Empty);
                    command.Parameters.AddWithValue("@EstaActivo", usuario.EstaActivo);
                    command.Parameters.AddWithValue("@EsAdmin", usuario.EsAdmin);
                    command.Parameters.AddWithValue("@Edad", usuario.Edad as object ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Altura", usuario.Altura as object ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Peso", usuario.Peso as object ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FotoPerfilURL", usuario.FotoPerfilURL as object ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FechaRegistro", usuario.FechaRegistro == default ? DateTime.Now : usuario.FechaRegistro);

                    var result = await command.ExecuteScalarAsync();
                    usuario.UsuarioID = Convert.ToInt32(result);
                    return usuario.UsuarioID;
                }
            }
        }
        
        public async Task UpdateAsync(Usuario usuario)
        {
            _logger?.LogInformation("🔍 REPOSITORY: Iniciando actualización de usuario {UsuarioID}", usuario.UsuarioID);
            _logger?.LogInformation("🔍 REPOSITORY: Datos a actualizar - EsAdmin: {EsAdmin}, EstaActivo: {EstaActivo}", 
                usuario.EsAdmin, usuario.EstaActivo);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                _logger?.LogInformation("🔗 REPOSITORY: Conexión a BD abierta");

                string query = @"UPDATE Usuarios
                               SET Nombre = @Nombre,
                                   Apellido = @Apellido,
                                   Email = @Email,
                                   Password = @Password,
                                   EstaActivo = @EstaActivo,
                                   EsAdmin = @EsAdmin,
                                   Edad = @Edad,
                                   Altura = @Altura,
                                   Peso = @Peso,
                                   FotoPerfilURL = @FotoPerfilURL
                               WHERE UsuarioID = @Id";

                _logger?.LogInformation("🔍 REPOSITORY: Query SQL: {Query}", query);

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", usuario.UsuarioID);
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre ?? string.Empty);
                    command.Parameters.AddWithValue("@Apellido", usuario.Apellido ?? string.Empty);
                    command.Parameters.AddWithValue("@Email", usuario.Email?.Trim().ToLower() ?? string.Empty);
                    command.Parameters.AddWithValue("@Password", usuario.Password ?? string.Empty);
                    command.Parameters.AddWithValue("@EstaActivo", usuario.EstaActivo);
                    command.Parameters.AddWithValue("@EsAdmin", usuario.EsAdmin);
                    command.Parameters.AddWithValue("@Edad", usuario.Edad as object ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Altura", usuario.Altura as object ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Peso", usuario.Peso as object ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FotoPerfilURL", usuario.FotoPerfilURL as object ?? DBNull.Value);

                    _logger?.LogInformation("🔍 REPOSITORY: Parámetros - UsuarioID: {UsuarioID}, EsAdmin: {EsAdmin}, EstaActivo: {EstaActivo}", 
                        usuario.UsuarioID, usuario.EsAdmin, usuario.EstaActivo);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    
                    _logger?.LogInformation("🔍 REPOSITORY: Filas afectadas: {RowsAffected}", rowsAffected);

                    if (rowsAffected == 0)
                    {
                        _logger?.LogWarning("⚠️ REPOSITORY: No se actualizó ninguna fila para usuario {UsuarioID}", usuario.UsuarioID);
                        throw new InvalidOperationException($"No se pudo actualizar el usuario con ID {usuario.UsuarioID}");
                    }
                    else
                    {
                        _logger?.LogInformation("✅ REPOSITORY: Usuario {UsuarioID} actualizado exitosamente", usuario.UsuarioID);
                    }
                }
            }

            // 🔍 VERIFICACIÓN ADICIONAL: Leer el usuario de vuelta para confirmar cambios
            _logger?.LogInformation("🔍 REPOSITORY: Verificando cambios en BD...");
            var usuarioVerificado = await GetByIdAsync(usuario.UsuarioID);
            if (usuarioVerificado != null)
            {
                _logger?.LogInformation("🔍 REPOSITORY: Usuario verificado - EsAdmin: {EsAdmin}, EstaActivo: {EstaActivo}", 
                    usuarioVerificado.EsAdmin, usuarioVerificado.EstaActivo);
                
                if (usuarioVerificado.EsAdmin != usuario.EsAdmin)
                {
                    _logger?.LogError("🚨 REPOSITORY: ERROR - EsAdmin no se actualizó. Esperado: {Esperado}, Actual: {Actual}", 
                        usuario.EsAdmin, usuarioVerificado.EsAdmin);
                }
                if (usuarioVerificado.EstaActivo != usuario.EstaActivo)
                {
                    _logger?.LogError("🚨 REPOSITORY: ERROR - EstaActivo no se actualizó. Esperado: {Esperado}, Actual: {Actual}", 
                        usuario.EstaActivo, usuarioVerificado.EstaActivo);
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Eliminar UsuarioLogros
                        await ExecuteDeleteCommand(connection, transaction, 
                            "DELETE FROM UsuarioLogros WHERE UsuarioID = @Id", id);

                        // 2. Eliminar RutinasCompletadas
                        await ExecuteDeleteCommand(connection, transaction, 
                            "DELETE FROM RutinasCompletadas WHERE UsuarioID = @Id", id);

                        // 3. Eliminar Comentarios (si existe la tabla)
                        await ExecuteDeleteCommand(connection, transaction, 
                            "DELETE FROM Comentarios WHERE UsuarioID = @Id", id);

                        // 4. Eliminar Mediciones (si existe la tabla)
                        await ExecuteDeleteCommand(connection, transaction, 
                            "DELETE FROM Mediciones WHERE UsuarioID = @Id", id);

                        // 5. Eliminar UsuarioRoles (si existe la tabla)
                        await ExecuteDeleteCommand(connection, transaction, 
                            "DELETE FROM UsuarioRoles WHERE UsuarioID = @Id", id);

                        // 6. Eliminar registros de Entrenamientos creados por el usuario (opcional)
                        // Puedes comentar esta línea si quieres mantener los entrenamientos
                        await ExecuteDeleteCommand(connection, transaction, 
                            "UPDATE Entrenamientos SET AutorID = NULL WHERE AutorID = @Id", id);

                        // 7. FINALMENTE eliminar el Usuario
                        await ExecuteDeleteCommand(connection, transaction, 
                            "DELETE FROM Usuarios WHERE UsuarioID = @Id", id);

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        // Método helper para ejecutar comandos de eliminación
        private async Task ExecuteDeleteCommand(SqlConnection connection, SqlTransaction transaction, string query, int id)
        {
            try
            {
                using (var command = new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                // Si la tabla no existe, ignorar el error (para compatibilidad)
                if (ex.Number != 208) // Invalid object name
                {
                    throw;
                }
            }
        }

        // Métodos adicionales útiles para administración

        public async Task<int> GetTotalUsersCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Usuarios";
                using (var command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<int> GetActiveUsersCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Usuarios WHERE EstaActivo = 1";
                using (var command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<int> GetAdminUsersCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Usuarios WHERE EsAdmin = 1 AND EstaActivo = 1";
                using (var command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<int> GetUsersRegisteredTodayAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT COUNT(*) FROM Usuarios 
                               WHERE CAST(FechaRegistro AS DATE) = CAST(GETDATE() AS DATE)";
                using (var command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<int> GetUsersRegisteredThisMonthAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT COUNT(*) FROM Usuarios 
                               WHERE MONTH(FechaRegistro) = MONTH(GETDATE()) 
                               AND YEAR(FechaRegistro) = YEAR(GETDATE())";
                using (var command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Usuarios WHERE UsuarioID = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count > 0;
                }
            }
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Usuarios WHERE Email = @Email";
                
                if (excludeUserId.HasValue)
                {
                    query += " AND UsuarioID != @ExcludeId";
                }
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email.Trim().ToLower());
                    if (excludeUserId.HasValue)
                    {
                        command.Parameters.AddWithValue("@ExcludeId", excludeUserId.Value);
                    }
                    
                    var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count > 0;
                }
            }
        }

        public async Task UpdatePasswordAsync(int userId, string newPassword)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"UPDATE Usuarios 
                                SET Password = @Password,
                                    ResetPasswordToken = NULL,
                                    ResetPasswordExpires = NULL
                                WHERE UsuarioID = @UserId";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Password", newPassword ?? string.Empty);
                    
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}