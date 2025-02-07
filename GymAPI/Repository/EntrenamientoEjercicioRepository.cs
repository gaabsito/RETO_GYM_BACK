using Microsoft.Data.SqlClient;
using GymAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymAPI.Repositories
{
    public class EntrenamientoEjercicioRepository : IEntrenamientoEjercicioRepository
    {
        private readonly string _connectionString;

        public EntrenamientoEjercicioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 🔹 Obtener todos los ejercicios asignados a entrenamientos
        public async Task<List<EntrenamientoEjercicio>> GetAllAsync()
        {
            var lista = new List<EntrenamientoEjercicio>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT EntrenamientoID, EjercicioID, Series, Repeticiones, DescansoSegundos, Notas FROM EntrenamientoEjercicios";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new EntrenamientoEjercicio
                        {
                            EntrenamientoID = reader.GetInt32(0),
                            EjercicioID = reader.GetInt32(1),
                            Series = reader.GetInt32(2),
                            Repeticiones = reader.GetInt32(3),
                            DescansoSegundos = reader.GetInt32(4),
                            Notas = reader.IsDBNull(5) ? null : reader.GetString(5)
                        });
                    }
                }
            }
            return lista;
        }

        // 🔹 Obtener ejercicios por entrenamiento
        public async Task<List<EntrenamientoEjercicio>> GetByEntrenamientoAsync(int entrenamientoID)
        {
            var lista = new List<EntrenamientoEjercicio>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT EntrenamientoID, EjercicioID, Series, Repeticiones, DescansoSegundos, Notas " +
                               "FROM EntrenamientoEjercicios WHERE EntrenamientoID = @EntrenamientoID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EntrenamientoID", entrenamientoID);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new EntrenamientoEjercicio
                            {
                                EntrenamientoID = reader.GetInt32(0),
                                EjercicioID = reader.GetInt32(1),
                                Series = reader.GetInt32(2),
                                Repeticiones = reader.GetInt32(3),
                                DescansoSegundos = reader.GetInt32(4),
                                Notas = reader.IsDBNull(5) ? null : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return lista;
        }

        // 🔹 Obtener un solo ejercicio dentro de un entrenamiento específico
        public async Task<EntrenamientoEjercicio?> GetByIdAsync(int entrenamientoID, int ejercicioID)
        {
            EntrenamientoEjercicio? entrenamientoEjercicio = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT EntrenamientoID, EjercicioID, Series, Repeticiones, DescansoSegundos, Notas " +
                               "FROM EntrenamientoEjercicios WHERE EntrenamientoID = @EntrenamientoID AND EjercicioID = @EjercicioID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EntrenamientoID", entrenamientoID);
                    command.Parameters.AddWithValue("@EjercicioID", ejercicioID);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            entrenamientoEjercicio = new EntrenamientoEjercicio
                            {
                                EntrenamientoID = reader.GetInt32(0),
                                EjercicioID = reader.GetInt32(1),
                                Series = reader.GetInt32(2),
                                Repeticiones = reader.GetInt32(3),
                                DescansoSegundos = reader.GetInt32(4),
                                Notas = reader.IsDBNull(5) ? null : reader.GetString(5)
                            };
                        }
                    }
                }
            }
            return entrenamientoEjercicio;
        }

        // 🔹 Agregar un nuevo ejercicio a un entrenamiento
        public async Task AddAsync(EntrenamientoEjercicio entrenamientoEjercicio)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO EntrenamientoEjercicios (EntrenamientoID, EjercicioID, Series, Repeticiones, DescansoSegundos, Notas) " +
                               "VALUES (@EntrenamientoID, @EjercicioID, @Series, @Repeticiones, @DescansoSegundos, @Notas)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EntrenamientoID", entrenamientoEjercicio.EntrenamientoID);
                    command.Parameters.AddWithValue("@EjercicioID", entrenamientoEjercicio.EjercicioID);
                    command.Parameters.AddWithValue("@Series", entrenamientoEjercicio.Series);
                    command.Parameters.AddWithValue("@Repeticiones", entrenamientoEjercicio.Repeticiones);
                    command.Parameters.AddWithValue("@DescansoSegundos", entrenamientoEjercicio.DescansoSegundos);
                    command.Parameters.AddWithValue("@Notas", (object?)entrenamientoEjercicio.Notas ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // 🔹 Actualizar un ejercicio dentro de un entrenamiento
        public async Task UpdateAsync(int entrenamientoID, int ejercicioID, EntrenamientoEjercicio entrenamientoEjercicio)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE EntrenamientoEjercicios SET Series = @Series, Repeticiones = @Repeticiones, " +
                               "DescansoSegundos = @DescansoSegundos, Notas = @Notas " +
                               "WHERE EntrenamientoID = @EntrenamientoID AND EjercicioID = @EjercicioID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EntrenamientoID", entrenamientoID);
                    command.Parameters.AddWithValue("@EjercicioID", ejercicioID);
                    command.Parameters.AddWithValue("@Series", entrenamientoEjercicio.Series);
                    command.Parameters.AddWithValue("@Repeticiones", entrenamientoEjercicio.Repeticiones);
                    command.Parameters.AddWithValue("@DescansoSegundos", entrenamientoEjercicio.DescansoSegundos);
                    command.Parameters.AddWithValue("@Notas", (object?)entrenamientoEjercicio.Notas ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // 🔹 Eliminar un ejercicio de un entrenamiento
        public async Task RemoveAsync(int entrenamientoID, int ejercicioID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM EntrenamientoEjercicios WHERE EntrenamientoID = @EntrenamientoID AND EjercicioID = @EjercicioID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EntrenamientoID", entrenamientoID);
                    command.Parameters.AddWithValue("@EjercicioID", ejercicioID);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
