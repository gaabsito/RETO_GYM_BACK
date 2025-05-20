// GymAPI/Repository/UsuarioLogroRepository.cs
using Microsoft.Data.SqlClient;
using GymAPI.Models;

namespace GymAPI.Repositories
{
    public class UsuarioLogroRepository : IUsuarioLogroRepository
    {
        private readonly string _connectionString;

        public UsuarioLogroRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<UsuarioLogro>> GetByUsuarioIdAsync(int usuarioId)
        {
            var usuarioLogros = new List<UsuarioLogro>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT ul.UsuarioLogroID, ul.UsuarioID, ul.LogroID, 
                               ul.FechaDesbloqueo, ul.ProgresoActual, ul.Desbloqueado,
                               l.Nombre, l.Descripcion, l.Icono, l.Color, l.Experiencia, 
                               l.Categoria, l.CondicionLogro, l.ValorMeta, l.Secreto
                               FROM UsuarioLogros ul
                               JOIN Logros l ON ul.LogroID = l.LogroID
                               WHERE ul.UsuarioID = @UsuarioId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var usuarioLogro = new UsuarioLogro
                            {
                                UsuarioLogroID = reader.GetInt32(0),
                                UsuarioID = reader.GetInt32(1),
                                LogroID = reader.GetInt32(2),
                                FechaDesbloqueo = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                                ProgresoActual = reader.GetInt32(4),
                                Desbloqueado = reader.GetBoolean(5),
                                Logro = new Logro
                                {
                                    LogroID = reader.GetInt32(2),
                                    Nombre = reader.GetString(6),
                                    Descripcion = reader.GetString(7),
                                    Icono = reader.GetString(8),
                                    Color = reader.GetString(9),
                                    Experiencia = reader.GetInt32(10),
                                    Categoria = reader.GetString(11),
                                    CondicionLogro = reader.GetString(12),
                                    ValorMeta = reader.GetInt32(13),
                                    Secreto = reader.GetBoolean(14)
                                }
                            };
                            usuarioLogros.Add(usuarioLogro);
                        }
                    }
                }
            }
            return usuarioLogros;
        }

        // Implementar los métodos restantes...

        public async Task UpdateProgresoAsync(int usuarioId, int logroId, int progreso, bool desbloqueado)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                // Verificar si el usuario ya tiene una relación con ese logro
                string checkQuery = @"SELECT UsuarioLogroID FROM UsuarioLogros 
                                     WHERE UsuarioID = @UsuarioId AND LogroID = @LogroId";
                
                using (var checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    checkCommand.Parameters.AddWithValue("@LogroId", logroId);
                    
                    var existingId = await checkCommand.ExecuteScalarAsync();
                    
                    if (existingId != null) // Actualizar existente
                    {
                        string updateQuery = @"UPDATE UsuarioLogros 
                                             SET ProgresoActual = @Progreso, 
                                                 Desbloqueado = @Desbloqueado,
                                                 FechaDesbloqueo = CASE WHEN @Desbloqueado = 1 AND Desbloqueado = 0 
                                                                      THEN GETDATE() ELSE FechaDesbloqueo END
                                             WHERE UsuarioID = @UsuarioId AND LogroID = @LogroId";
                        
                        using (var updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@UsuarioId", usuarioId);
                            updateCommand.Parameters.AddWithValue("@LogroId", logroId);
                            updateCommand.Parameters.AddWithValue("@Progreso", progreso);
                            updateCommand.Parameters.AddWithValue("@Desbloqueado", desbloqueado);
                            
                            await updateCommand.ExecuteNonQueryAsync();
                        }
                    }
                    else // Crear nuevo
                    {
                        string insertQuery = @"INSERT INTO UsuarioLogros (UsuarioID, LogroID, 
                                             ProgresoActual, Desbloqueado, FechaDesbloqueo)
                                             VALUES (@UsuarioId, @LogroId, @Progreso, @Desbloqueado, 
                                             CASE WHEN @Desbloqueado = 1 THEN GETDATE() ELSE NULL END)";
                        
                        using (var insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@UsuarioId", usuarioId);
                            insertCommand.Parameters.AddWithValue("@LogroId", logroId);
                            insertCommand.Parameters.AddWithValue("@Progreso", progreso);
                            insertCommand.Parameters.AddWithValue("@Desbloqueado", desbloqueado);
                            
                            await insertCommand.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }
    }
}