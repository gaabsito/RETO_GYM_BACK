using Microsoft.Data.SqlClient;
using GymAPI.Models;

namespace GymAPI.Repositories
{
    public class EjercicioRepository : IEjercicioRepository
    {
        private readonly string _connectionString;

        public EjercicioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Ejercicio>> GetAllAsync()
        {
            var ejercicios = new List<Ejercicio>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT EjercicioID, Nombre, Descripcion, GrupoMuscular, 
                               ImagenURL, VideoURL, EquipamientoNecesario 
                               FROM Ejercicios 
                               ORDER BY Nombre";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ejercicios.Add(new Ejercicio
                        {
                            EjercicioID = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
                            GrupoMuscular = reader.IsDBNull(3) ? null : reader.GetString(3),
                            ImagenURL = reader.IsDBNull(4) ? null : reader.GetString(4),
                            VideoURL = reader.IsDBNull(5) ? null : reader.GetString(5),
                            EquipamientoNecesario = reader.GetBoolean(6)
                        });
                    }
                }
            }
            return ejercicios;
        }

        public async Task<Ejercicio?> GetByIdAsync(int id)
        {
            Ejercicio? ejercicio = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT EjercicioID, Nombre, Descripcion, GrupoMuscular, 
                               ImagenURL, VideoURL, EquipamientoNecesario 
                               FROM Ejercicios 
                               WHERE EjercicioID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            ejercicio = new Ejercicio
                            {
                                EjercicioID = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
                                GrupoMuscular = reader.IsDBNull(3) ? null : reader.GetString(3),
                                ImagenURL = reader.IsDBNull(4) ? null : reader.GetString(4),
                                VideoURL = reader.IsDBNull(5) ? null : reader.GetString(5),
                                EquipamientoNecesario = reader.GetBoolean(6)
                            };
                        }
                    }
                }
            }
            return ejercicio;
        }

        public async Task<List<Ejercicio>> GetByGrupoMuscularAsync(string grupoMuscular)
        {
            var ejercicios = new List<Ejercicio>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT EjercicioID, Nombre, Descripcion, GrupoMuscular, 
                               ImagenURL, VideoURL, EquipamientoNecesario 
                               FROM Ejercicios 
                               WHERE GrupoMuscular = @GrupoMuscular
                               ORDER BY Nombre";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GrupoMuscular", grupoMuscular);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ejercicios.Add(new Ejercicio
                            {
                                EjercicioID = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
                                GrupoMuscular = reader.IsDBNull(3) ? null : reader.GetString(3),
                                ImagenURL = reader.IsDBNull(4) ? null : reader.GetString(4),
                                VideoURL = reader.IsDBNull(5) ? null : reader.GetString(5),
                                EquipamientoNecesario = reader.GetBoolean(6)
                            });
                        }
                    }
                }
            }
            return ejercicios;
        }

        public async Task<List<Ejercicio>> GetByEquipamientoAsync(bool requiereEquipamiento)
        {
            var ejercicios = new List<Ejercicio>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT EjercicioID, Nombre, Descripcion, GrupoMuscular, 
                               ImagenURL, VideoURL, EquipamientoNecesario 
                               FROM Ejercicios 
                               WHERE EquipamientoNecesario = @EquipamientoNecesario
                               ORDER BY Nombre";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EquipamientoNecesario", requiereEquipamiento);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ejercicios.Add(new Ejercicio
                            {
                                EjercicioID = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
                                GrupoMuscular = reader.IsDBNull(3) ? null : reader.GetString(3),
                                ImagenURL = reader.IsDBNull(4) ? null : reader.GetString(4),
                                VideoURL = reader.IsDBNull(5) ? null : reader.GetString(5),
                                EquipamientoNecesario = reader.GetBoolean(6)
                            });
                        }
                    }
                }
            }
            return ejercicios;
        }

        public async Task<List<string>> GetGruposMusculares()
        {
            var grupos = new List<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT DISTINCT GrupoMuscular 
                               FROM Ejercicios 
                               WHERE GrupoMuscular IS NOT NULL 
                               ORDER BY GrupoMuscular";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        grupos.Add(reader.GetString(0));
                    }
                }
            }
            return grupos;
        }

        public async Task<List<Ejercicio>> SearchAsync(string searchTerm)
        {
            var ejercicios = new List<Ejercicio>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT EjercicioID, Nombre, Descripcion, GrupoMuscular, 
                               ImagenURL, VideoURL, EquipamientoNecesario 
                               FROM Ejercicios 
                               WHERE Nombre LIKE @SearchTerm 
                                  OR Descripcion LIKE @SearchTerm 
                                  OR GrupoMuscular LIKE @SearchTerm
                               ORDER BY Nombre";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ejercicios.Add(new Ejercicio
                            {
                                EjercicioID = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
                                GrupoMuscular = reader.IsDBNull(3) ? null : reader.GetString(3),
                                ImagenURL = reader.IsDBNull(4) ? null : reader.GetString(4),
                                VideoURL = reader.IsDBNull(5) ? null : reader.GetString(5),
                                EquipamientoNecesario = reader.GetBoolean(6)
                            });
                        }
                    }
                }
            }
            return ejercicios;
        }

        public async Task<int> AddAsync(Ejercicio ejercicio)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Ejercicios (Nombre, Descripcion, GrupoMuscular, 
                               ImagenURL, VideoURL, EquipamientoNecesario) 
                               VALUES (@Nombre, @Descripcion, @GrupoMuscular, 
                               @ImagenURL, @VideoURL, @EquipamientoNecesario);
                               SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", ejercicio.Nombre ?? string.Empty);
                    command.Parameters.AddWithValue("@Descripcion", (object?)ejercicio.Descripcion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@GrupoMuscular", (object?)ejercicio.GrupoMuscular ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ImagenURL", (object?)ejercicio.ImagenURL ?? DBNull.Value);
                    command.Parameters.AddWithValue("@VideoURL", (object?)ejercicio.VideoURL ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EquipamientoNecesario", ejercicio.EquipamientoNecesario);

                    var result = await command.ExecuteScalarAsync();
                    ejercicio.EjercicioID = Convert.ToInt32(result);
                    return ejercicio.EjercicioID;
                }
            }
        }

        public async Task UpdateAsync(Ejercicio ejercicio)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"UPDATE Ejercicios 
                               SET Nombre = @Nombre, 
                                   Descripcion = @Descripcion, 
                                   GrupoMuscular = @GrupoMuscular, 
                                   ImagenURL = @ImagenURL, 
                                   VideoURL = @VideoURL, 
                                   EquipamientoNecesario = @EquipamientoNecesario 
                               WHERE EjercicioID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", ejercicio.EjercicioID);
                    command.Parameters.AddWithValue("@Nombre", ejercicio.Nombre ?? string.Empty);
                    command.Parameters.AddWithValue("@Descripcion", (object?)ejercicio.Descripcion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@GrupoMuscular", (object?)ejercicio.GrupoMuscular ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ImagenURL", (object?)ejercicio.ImagenURL ?? DBNull.Value);
                    command.Parameters.AddWithValue("@VideoURL", (object?)ejercicio.VideoURL ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EquipamientoNecesario", ejercicio.EquipamientoNecesario);

                    await command.ExecuteNonQueryAsync();
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
                        // 1. Eliminar relaciones con entrenamientos
                        await ExecuteDeleteCommand(connection, transaction, 
                            "DELETE FROM EntrenamientoEjercicios WHERE EjercicioID = @Id", id);

                        // 2. Eliminar el ejercicio
                        await ExecuteDeleteCommand(connection, transaction, 
                            "DELETE FROM Ejercicios WHERE EjercicioID = @Id", id);

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
                if (ex.Number != 208) // Invalid object name
                {
                    throw;
                }
            }
        }

        // Métodos adicionales para estadísticas y administración

        public async Task<int> GetTotalCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Ejercicios";
                using (var command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<int> GetCountByGrupoMuscularAsync(string grupoMuscular)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Ejercicios WHERE GrupoMuscular = @GrupoMuscular";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GrupoMuscular", grupoMuscular);
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<int> GetCountWithEquipmentAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Ejercicios WHERE EquipamientoNecesario = 1";
                using (var command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<int> GetCountWithoutEquipmentAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Ejercicios WHERE EquipamientoNecesario = 0";
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
                string query = "SELECT COUNT(*) FROM Ejercicios WHERE EjercicioID = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count > 0;
                }
            }
        }

        public async Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Ejercicios WHERE Nombre = @Nombre";
                
                if (excludeId.HasValue)
                {
                    query += " AND EjercicioID != @ExcludeId";
                }
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    if (excludeId.HasValue)
                    {
                        command.Parameters.AddWithValue("@ExcludeId", excludeId.Value);
                    }
                    
                    var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count > 0;
                }
            }
        }

        // Métodos para obtener ejercicios más utilizados
        public async Task<List<(int EjercicioID, string Nombre, int Veces)>> GetMostUsedAsync(int limit = 10)
        {
            var ejercicios = new List<(int, string, int)>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = $@"
                    SELECT TOP {limit} 
                        e.EjercicioID, 
                        e.Nombre, 
                        COUNT(ee.EjercicioID) as Veces
                    FROM Ejercicios e
                    LEFT JOIN EntrenamientoEjercicios ee ON e.EjercicioID = ee.EjercicioID
                    GROUP BY e.EjercicioID, e.Nombre
                    ORDER BY COUNT(ee.EjercicioID) DESC";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ejercicios.Add((
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetInt32(2)
                        ));
                    }
                }
            }
            return ejercicios;
        }
    }
}