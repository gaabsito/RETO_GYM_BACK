using Microsoft.Data.SqlClient;
using GymAPI.Models;

namespace GymAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<Usuario>> GetAllAsync()
        {
            var usuarios = new List<Usuario>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT UsuarioID, Nombre, Apellido, Email, FechaRegistro, EstaActivo FROM Usuarios";
                using (var command = new SqlCommand(query, connection))
                {
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
                                EstaActivo = reader.GetBoolean(5)
                            };
                            usuarios.Add(usuario);
                        }
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
                      FechaRegistro, EstaActivo FROM Usuarios WHERE UsuarioID = @Id";
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
                                EstaActivo = reader.GetBoolean(6)
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            Usuario? usuario = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT UsuarioID, Nombre, Apellido, Email, Password, 
                        FechaRegistro, EstaActivo, ResetPasswordToken, ResetPasswordExpires 
                        FROM Usuarios WHERE Email = @Email";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
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
                                ResetPasswordToken = reader.IsDBNull(7) ? null : reader.GetString(7),
                                ResetPasswordExpires = reader.IsDBNull(8) ? null : reader.GetDateTime(8)
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
            Usuario? usuario = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT UsuarioID, Nombre, Apellido, Email, Password, 
                        FechaRegistro, EstaActivo, ResetPasswordToken, ResetPasswordExpires 
                        FROM Usuarios WHERE ResetPasswordToken = @Token";
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
                                ResetPasswordToken = reader.IsDBNull(7) ? null : reader.GetString(7),
                                ResetPasswordExpires = reader.IsDBNull(8) ? null : reader.GetDateTime(8)
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
                string query = @"INSERT INTO Usuarios (Nombre, Apellido, Email, Password, EstaActivo) 
                        VALUES (@Nombre, @Apellido, @Email, @Password, @EstaActivo);
                        SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Password", usuario.Password);
                    command.Parameters.AddWithValue("@EstaActivo", usuario.EstaActivo);

                    var result = await command.ExecuteScalarAsync();
                    usuario.UsuarioID = Convert.ToInt32(result);
                    return usuario.UsuarioID;
                }
            }
        }
        public async Task UpdateAsync(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Agregar aquí las columnas que quieras actualizar (Edad, Altura, Peso)
                string query = @"
            UPDATE Usuarios
            SET
                Nombre = @Nombre,
                Apellido = @Apellido,
                Email = @Email,
                Password = @Password,
                EstaActivo = @EstaActivo,
                Edad = @Edad,
                Altura = @Altura,
                Peso = @Peso
            WHERE UsuarioID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    // Asignar los parámetros
                    command.Parameters.AddWithValue("@Id", usuario.UsuarioID);
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Password", usuario.Password);
                    command.Parameters.AddWithValue("@EstaActivo", usuario.EstaActivo);

                    // Manejar nulos para Edad, Altura, Peso (si tus propiedades son int?/float?)
                    command.Parameters.AddWithValue("@Edad", usuario.Edad ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Altura", usuario.Altura ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Peso", usuario.Peso ?? (object)DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM Usuarios WHERE UsuarioID = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}