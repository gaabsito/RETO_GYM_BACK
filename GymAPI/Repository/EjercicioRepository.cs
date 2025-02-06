using Microsoft.Data.SqlClient;
using GymAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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
                string query = "SELECT EjercicioID, Nombre, Descripcion, GrupoMuscular, ImagenURL, EquipamientoNecesario FROM Ejercicios";

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
                            EquipamientoNecesario = reader.GetBoolean(5)
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
                string query = "SELECT EjercicioID, Nombre, Descripcion, GrupoMuscular, ImagenURL, EquipamientoNecesario FROM Ejercicios WHERE EjercicioID = @Id";

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
                                EquipamientoNecesario = reader.GetBoolean(5)
                            };
                        }
                    }
                }
            }
            return ejercicio;
        }

        public async Task AddAsync(Ejercicio ejercicio)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Ejercicios (Nombre, Descripcion, GrupoMuscular, ImagenURL, EquipamientoNecesario) " +
                               "VALUES (@Nombre, @Descripcion, @GrupoMuscular, @ImagenURL, @EquipamientoNecesario)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", ejercicio.Nombre);
                    command.Parameters.AddWithValue("@Descripcion", (object?)ejercicio.Descripcion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@GrupoMuscular", (object?)ejercicio.GrupoMuscular ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ImagenURL", (object?)ejercicio.ImagenURL ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EquipamientoNecesario", ejercicio.EquipamientoNecesario);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Ejercicio ejercicio)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE Ejercicios SET Nombre = @Nombre, Descripcion = @Descripcion, GrupoMuscular = @GrupoMuscular, ImagenURL = @ImagenURL, EquipamientoNecesario = @EquipamientoNecesario " +
                               "WHERE EjercicioID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", ejercicio.EjercicioID);
                    command.Parameters.AddWithValue("@Nombre", ejercicio.Nombre);
                    command.Parameters.AddWithValue("@Descripcion", (object?)ejercicio.Descripcion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@GrupoMuscular", (object?)ejercicio.GrupoMuscular ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ImagenURL", (object?)ejercicio.ImagenURL ?? DBNull.Value);
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
                string query = "DELETE FROM Ejercicios WHERE EjercicioID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
