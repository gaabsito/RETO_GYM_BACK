// GymAPI/Repository/LogroRepository.cs
using Microsoft.Data.SqlClient;
using GymAPI.Models;
namespace GymAPI.Repositories
{
    public class LogroRepository : ILogroRepository
    {
        private readonly string _connectionString;
        public LogroRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<Logro>> GetAllAsync()
        {
            var logros = new List<Logro>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT LogroID, Nombre, Descripcion, Icono, Color,
                         Experiencia, Categoria, CondicionLogro, ValorMeta, Secreto
                         FROM Logros";
                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        logros.Add(new Logro
                        {
                            LogroID = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                            Icono = reader.GetString(3),
                            Color = reader.GetString(4),
                            Experiencia = reader.GetInt32(5),
                            Categoria = reader.GetString(6),
                            CondicionLogro = reader.GetString(7),
                            ValorMeta = reader.GetInt32(8),
                            Secreto = reader.GetBoolean(9)
                        });
                    }
                }
            }
            return logros;
        }
        public async Task<Logro?> GetByIdAsync(int id)
        {
            Logro? logro = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT LogroID, Nombre, Descripcion, Icono, Color,
                         Experiencia, Categoria, CondicionLogro, ValorMeta, Secreto
                         FROM Logros WHERE LogroID = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            logro = new Logro
                            {
                                LogroID = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Descripcion = reader.GetString(2),
                                Icono = reader.GetString(3),
                                Color = reader.GetString(4),
                                Experiencia = reader.GetInt32(5),
                                Categoria = reader.GetString(6),
                                CondicionLogro = reader.GetString(7),
                                ValorMeta = reader.GetInt32(8),
                                Secreto = reader.GetBoolean(9)
                            };
                        }
                    }
                }
            }
            return logro;
        }
        public async Task<List<Logro>> GetByCategoriaAsync(string categoria)
        {
            var logros = new List<Logro>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT LogroID, Nombre, Descripcion, Icono, Color,
                         Experiencia, Categoria, CondicionLogro, ValorMeta, Secreto
                         FROM Logros WHERE Categoria = @Categoria";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Categoria", categoria);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            logros.Add(new Logro
                            {
                                LogroID = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Descripcion = reader.GetString(2),
                                Icono = reader.GetString(3),
                                Color = reader.GetString(4),
                                Experiencia = reader.GetInt32(5),
                                Categoria = reader.GetString(6),
                                CondicionLogro = reader.GetString(7),
                                ValorMeta = reader.GetInt32(8),
                                Secreto = reader.GetBoolean(9)
                            });
                        }
                    }
                }
            }
            return logros;
        }
        
        // Implementación de los métodos faltantes
        public async Task<int> AddAsync(Logro logro)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Logros 
                               (Nombre, Descripcion, Icono, Color, Experiencia, Categoria, CondicionLogro, ValorMeta, Secreto)
                               VALUES (@Nombre, @Descripcion, @Icono, @Color, @Experiencia, @Categoria, @CondicionLogro, @ValorMeta, @Secreto);
                               SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", logro.Nombre);
                    command.Parameters.AddWithValue("@Descripcion", logro.Descripcion);
                    command.Parameters.AddWithValue("@Icono", logro.Icono);
                    command.Parameters.AddWithValue("@Color", logro.Color);
                    command.Parameters.AddWithValue("@Experiencia", logro.Experiencia);
                    command.Parameters.AddWithValue("@Categoria", logro.Categoria);
                    command.Parameters.AddWithValue("@CondicionLogro", logro.CondicionLogro);
                    command.Parameters.AddWithValue("@ValorMeta", logro.ValorMeta);
                    command.Parameters.AddWithValue("@Secreto", logro.Secreto);

                    // Ejecutar la consulta y obtener el ID generado
                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }

        public async Task UpdateAsync(Logro logro)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"UPDATE Logros 
                               SET Nombre = @Nombre, 
                                   Descripcion = @Descripcion, 
                                   Icono = @Icono, 
                                   Color = @Color, 
                                   Experiencia = @Experiencia, 
                                   Categoria = @Categoria, 
                                   CondicionLogro = @CondicionLogro, 
                                   ValorMeta = @ValorMeta, 
                                   Secreto = @Secreto
                               WHERE LogroID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", logro.LogroID);
                    command.Parameters.AddWithValue("@Nombre", logro.Nombre);
                    command.Parameters.AddWithValue("@Descripcion", logro.Descripcion);
                    command.Parameters.AddWithValue("@Icono", logro.Icono);
                    command.Parameters.AddWithValue("@Color", logro.Color);
                    command.Parameters.AddWithValue("@Experiencia", logro.Experiencia);
                    command.Parameters.AddWithValue("@Categoria", logro.Categoria);
                    command.Parameters.AddWithValue("@CondicionLogro", logro.CondicionLogro);
                    command.Parameters.AddWithValue("@ValorMeta", logro.ValorMeta);
                    command.Parameters.AddWithValue("@Secreto", logro.Secreto);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                // Primero verificamos si hay usuarios que tengan este logro
                // y eliminamos esas relaciones para evitar problemas de FK
                string deleteRelationsQuery = @"DELETE FROM UsuarioLogros WHERE LogroID = @Id";
                
                using (var command = new SqlCommand(deleteRelationsQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
                
                // Luego eliminamos el logro
                string deleteQuery = @"DELETE FROM Logros WHERE LogroID = @Id";
                
                using (var command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}