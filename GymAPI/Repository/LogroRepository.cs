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

        // Implementar los métodos restantes (Add, Update, Delete)...
    }
}