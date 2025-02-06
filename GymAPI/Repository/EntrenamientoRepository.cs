using Microsoft.Data.SqlClient;
using GymAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GymAPI.Repositories
{
    public class EntrenamientoRepository : IEntrenamientoRepository
    {
        private readonly string _connectionString;

        public EntrenamientoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Entrenamiento>> GetAllAsync()
        {
            var entrenamientos = new List<Entrenamiento>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT EntrenamientoID, Titulo, Descripcion, DuracionMinutos, Dificultad, FechaCreacion, Publico, AutorID FROM Entrenamientos";
                
                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        entrenamientos.Add(new Entrenamiento
                        {
                            EntrenamientoID = reader.GetInt32(0),
                            Titulo = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                            DuracionMinutos = reader.GetInt32(3),
                            Dificultad = reader.GetString(4), // Ahora es un string
                            FechaCreacion = reader.GetDateTime(5),
                            Publico = reader.GetBoolean(6),
                            AutorID = reader.IsDBNull(7) ? null : reader.GetInt32(7)
                        });
                    }
                }
            }
            return entrenamientos;
        }

        public async Task<Entrenamiento?> GetByIdAsync(int id)
        {
            Entrenamiento? entrenamiento = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT EntrenamientoID, Titulo, Descripcion, DuracionMinutos, Dificultad, FechaCreacion, Publico, AutorID FROM Entrenamientos WHERE EntrenamientoID = @Id";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            entrenamiento = new Entrenamiento
                            {
                                EntrenamientoID = reader.GetInt32(0),
                                Titulo = reader.GetString(1),
                                Descripcion = reader.GetString(2),
                                DuracionMinutos = reader.GetInt32(3),
                                Dificultad = reader.GetString(4), // Ahora es un string
                                FechaCreacion = reader.GetDateTime(5),
                                Publico = reader.GetBoolean(6),
                                AutorID = reader.IsDBNull(7) ? null : reader.GetInt32(7)
                            };
                        }
                    }
                }
            }
            return entrenamiento;
        }

        public async Task AddAsync(Entrenamiento entrenamiento)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Entrenamientos (Titulo, Descripcion, DuracionMinutos, Dificultad, Publico, AutorID) VALUES (@Titulo, @Descripcion, @DuracionMinutos, @Dificultad, @Publico, @AutorID)";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Titulo", entrenamiento.Titulo);
                    command.Parameters.AddWithValue("@Descripcion", entrenamiento.Descripcion);
                    command.Parameters.AddWithValue("@DuracionMinutos", entrenamiento.DuracionMinutos);
                    command.Parameters.AddWithValue("@Dificultad", entrenamiento.Dificultad); // Ahora solo insertamos el string
                    command.Parameters.AddWithValue("@Publico", entrenamiento.Publico);
                    command.Parameters.AddWithValue("@AutorID", (object?)entrenamiento.AutorID ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Entrenamiento entrenamiento)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE Entrenamientos SET Titulo = @Titulo, Descripcion = @Descripcion, DuracionMinutos = @DuracionMinutos, Dificultad = @Dificultad, Publico = @Publico, AutorID = @AutorID WHERE EntrenamientoID = @Id";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", entrenamiento.EntrenamientoID);
                    command.Parameters.AddWithValue("@Titulo", entrenamiento.Titulo);
                    command.Parameters.AddWithValue("@Descripcion", entrenamiento.Descripcion);
                    command.Parameters.AddWithValue("@DuracionMinutos", entrenamiento.DuracionMinutos);
                    command.Parameters.AddWithValue("@Dificultad", entrenamiento.Dificultad); // Ahora es un string
                    command.Parameters.AddWithValue("@Publico", entrenamiento.Publico);
                    command.Parameters.AddWithValue("@AutorID", (object?)entrenamiento.AutorID ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM Entrenamientos WHERE EntrenamientoID = @Id";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
