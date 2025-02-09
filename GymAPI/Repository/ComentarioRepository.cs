using Microsoft.Data.SqlClient;
using GymAPI.Models;

namespace GymAPI.Repositories
{
    public class ComentarioRepository : IComentarioRepository
    {
        private readonly string _connectionString;

        public ComentarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Comentario>> GetAllAsync()
        {
            var comentarios = new List<Comentario>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT ComentarioID, EntrenamientoID, UsuarioID, Contenido, 
                               Calificacion, FechaComentario FROM Comentarios";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        comentarios.Add(new Comentario
                        {
                            ComentarioID = reader.GetInt32(0),
                            EntrenamientoID = reader.GetInt32(1),
                            UsuarioID = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2),
                            Contenido = reader.GetString(3),
                            Calificacion = reader.GetInt32(4),
                            FechaComentario = reader.GetDateTime(5)
                        });
                    }
                }
            }
            return comentarios;
        }

        public async Task<Comentario?> GetByIdAsync(int id)
        {
            Comentario? comentario = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT ComentarioID, EntrenamientoID, UsuarioID, Contenido, 
                               Calificacion, FechaComentario FROM Comentarios 
                               WHERE ComentarioID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            comentario = new Comentario
                            {
                                ComentarioID = reader.GetInt32(0),
                                EntrenamientoID = reader.GetInt32(1),
                                UsuarioID = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2),
                                Contenido = reader.GetString(3),
                                Calificacion = reader.GetInt32(4),
                                FechaComentario = reader.GetDateTime(5)
                            };
                        }
                    }
                }
            }
            return comentario;
        }

        public async Task<List<Comentario>> GetByEntrenamientoIdAsync(int entrenamientoId)
        {
            var comentarios = new List<Comentario>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT ComentarioID, EntrenamientoID, UsuarioID, Contenido, 
                               Calificacion, FechaComentario FROM Comentarios 
                               WHERE EntrenamientoID = @EntrenamientoId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EntrenamientoId", entrenamientoId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            comentarios.Add(new Comentario
                            {
                                ComentarioID = reader.GetInt32(0),
                                EntrenamientoID = reader.GetInt32(1),
                                UsuarioID = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2),
                                Contenido = reader.GetString(3),
                                Calificacion = reader.GetInt32(4),
                                FechaComentario = reader.GetDateTime(5)
                            });
                        }
                    }
                }
            }
            return comentarios;
        }

        public async Task AddAsync(Comentario comentario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Comentarios (EntrenamientoID, UsuarioID, Contenido, 
                               Calificacion, FechaComentario) 
                               VALUES (@EntrenamientoID, @UsuarioID, @Contenido, 
                               @Calificacion, @FechaComentario)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EntrenamientoID", comentario.EntrenamientoID);
                    command.Parameters.AddWithValue("@UsuarioID", (object?)comentario.UsuarioID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Contenido", comentario.Contenido);
                    command.Parameters.AddWithValue("@Calificacion", comentario.Calificacion);
                    command.Parameters.AddWithValue("@FechaComentario", comentario.FechaComentario);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Comentario comentario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"UPDATE Comentarios 
                               SET Contenido = @Contenido, 
                                   Calificacion = @Calificacion 
                               WHERE ComentarioID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", comentario.ComentarioID);
                    command.Parameters.AddWithValue("@Contenido", comentario.Contenido);
                    command.Parameters.AddWithValue("@Calificacion", comentario.Calificacion);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM Comentarios WHERE ComentarioID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}