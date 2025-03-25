using Microsoft.Data.SqlClient;
using GymAPI.Models;

namespace GymAPI.Repositories
{
    public class MedicionRepository : IMedicionRepository
    {
        private readonly string _connectionString;

        public MedicionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Medicion>> GetAllAsync()
        {
            var mediciones = new List<Medicion>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT MedicionID, UsuarioID, Fecha, Peso, Altura, IMC, 
                                PorcentajeGrasa, CircunferenciaBrazo, CircunferenciaPecho, 
                                CircunferenciaCintura, CircunferenciaMuslo, Notas 
                                FROM Mediciones";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        mediciones.Add(new Medicion
                        {
                            MedicionID = reader.GetInt32(0),
                            UsuarioID = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Peso = reader.IsDBNull(3) ? null : (float?)reader.GetFloat(3),
                            Altura = reader.IsDBNull(4) ? null : (float?)reader.GetFloat(4),
                            IMC = reader.IsDBNull(5) ? null : (float?)reader.GetFloat(5),
                            PorcentajeGrasa = reader.IsDBNull(6) ? null : (float?)reader.GetFloat(6),
                            CircunferenciaBrazo = reader.IsDBNull(7) ? null : (float?)reader.GetFloat(7),
                            CircunferenciaPecho = reader.IsDBNull(8) ? null : (float?)reader.GetFloat(8),
                            CircunferenciaCintura = reader.IsDBNull(9) ? null : (float?)reader.GetFloat(9),
                            CircunferenciaMuslo = reader.IsDBNull(10) ? null : (float?)reader.GetFloat(10),
                            Notas = reader.IsDBNull(11) ? null : reader.GetString(11)
                        });
                    }
                }
            }
            return mediciones;
        }

        public async Task<Medicion?> GetByIdAsync(int id)
        {
            Medicion? medicion = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT MedicionID, UsuarioID, Fecha, Peso, Altura, IMC, 
                                PorcentajeGrasa, CircunferenciaBrazo, CircunferenciaPecho, 
                                CircunferenciaCintura, CircunferenciaMuslo, Notas 
                                FROM Mediciones WHERE MedicionID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            medicion = new Medicion
                            {
                                MedicionID = reader.GetInt32(0),
                                UsuarioID = reader.GetInt32(1),
                                Fecha = reader.GetDateTime(2),
                                Peso = reader.IsDBNull(3) ? null : (float?)reader.GetFloat(3),
                                Altura = reader.IsDBNull(4) ? null : (float?)reader.GetFloat(4),
                                IMC = reader.IsDBNull(5) ? null : (float?)reader.GetFloat(5),
                                PorcentajeGrasa = reader.IsDBNull(6) ? null : (float?)reader.GetFloat(6),
                                CircunferenciaBrazo = reader.IsDBNull(7) ? null : (float?)reader.GetFloat(7),
                                CircunferenciaPecho = reader.IsDBNull(8) ? null : (float?)reader.GetFloat(8),
                                CircunferenciaCintura = reader.IsDBNull(9) ? null : (float?)reader.GetFloat(9),
                                CircunferenciaMuslo = reader.IsDBNull(10) ? null : (float?)reader.GetFloat(10),
                                Notas = reader.IsDBNull(11) ? null : reader.GetString(11)
                            };
                        }
                    }
                }
            }
            return medicion;
        }

        public async Task<List<Medicion>> GetByUsuarioIdAsync(int usuarioId)
        {
            var mediciones = new List<Medicion>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT MedicionID, UsuarioID, Fecha, Peso, Altura, IMC, 
                                PorcentajeGrasa, CircunferenciaBrazo, CircunferenciaPecho, 
                                CircunferenciaCintura, CircunferenciaMuslo, Notas 
                                FROM Mediciones 
                                WHERE UsuarioID = @UsuarioId
                                ORDER BY Fecha DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            mediciones.Add(new Medicion
                            {
                                MedicionID = reader.GetInt32(0),
                                UsuarioID = reader.GetInt32(1),
                                Fecha = reader.GetDateTime(2),
                                Peso = reader.IsDBNull(3) ? null : (float?)reader.GetFloat(3),
                                Altura = reader.IsDBNull(4) ? null : (float?)reader.GetFloat(4),
                                IMC = reader.IsDBNull(5) ? null : (float?)reader.GetFloat(5),
                                PorcentajeGrasa = reader.IsDBNull(6) ? null : (float?)reader.GetFloat(6),
                                CircunferenciaBrazo = reader.IsDBNull(7) ? null : (float?)reader.GetFloat(7),
                                CircunferenciaPecho = reader.IsDBNull(8) ? null : (float?)reader.GetFloat(8),
                                CircunferenciaCintura = reader.IsDBNull(9) ? null : (float?)reader.GetFloat(9),
                                CircunferenciaMuslo = reader.IsDBNull(10) ? null : (float?)reader.GetFloat(10),
                                Notas = reader.IsDBNull(11) ? null : reader.GetString(11)
                            });
                        }
                    }
                }
            }
            return mediciones;
        }

        public async Task<List<MedicionResumen>> GetResumenByUsuarioIdAsync(int usuarioId)
        {
            var resumen = new List<MedicionResumen>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT UsuarioID, Anio, Mes, PesoPromedio, IMCPromedio, 
                                GrasaPromedio, CinturaPromedio 
                                FROM MedicionesMensuales 
                                WHERE UsuarioID = @UsuarioId
                                ORDER BY Anio, Mes";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            resumen.Add(new MedicionResumen
                            {
                                UsuarioID = reader.GetInt32(0),
                                Anio = reader.GetInt32(1),
                                Mes = reader.GetInt32(2),
                                PesoPromedio = reader.IsDBNull(3) ? null : (float?)reader.GetFloat(3),
                                IMCPromedio = reader.IsDBNull(4) ? null : (float?)reader.GetFloat(4),
                                GrasaPromedio = reader.IsDBNull(5) ? null : (float?)reader.GetFloat(5),
                                CinturaPromedio = reader.IsDBNull(6) ? null : (float?)reader.GetFloat(6)
                            });
                        }
                    }
                }
            }
            return resumen;
        }

        public async Task<int> AddAsync(Medicion medicion)
        {
            // Calcular IMC si se proporcionan peso y altura
            if (medicion.Peso.HasValue && medicion.Altura.HasValue && medicion.Altura.Value > 0)
            {
                // IMC = peso(kg) / (altura(m))²
                // Asumiendo que la altura está en metros
                medicion.IMC = medicion.Peso.Value / (medicion.Altura.Value * medicion.Altura.Value);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Mediciones (UsuarioID, Fecha, Peso, Altura, IMC, 
                                PorcentajeGrasa, CircunferenciaBrazo, CircunferenciaPecho, 
                                CircunferenciaCintura, CircunferenciaMuslo, Notas)
                                VALUES (@UsuarioID, @Fecha, @Peso, @Altura, @IMC, 
                                @PorcentajeGrasa, @CircunferenciaBrazo, @CircunferenciaPecho, 
                                @CircunferenciaCintura, @CircunferenciaMuslo, @Notas);
                                SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioID", medicion.UsuarioID);
                    command.Parameters.AddWithValue("@Fecha", medicion.Fecha);
                    command.Parameters.AddWithValue("@Peso", (object?)medicion.Peso ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Altura", (object?)medicion.Altura ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IMC", (object?)medicion.IMC ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PorcentajeGrasa", (object?)medicion.PorcentajeGrasa ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaBrazo", (object?)medicion.CircunferenciaBrazo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaPecho", (object?)medicion.CircunferenciaPecho ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaCintura", (object?)medicion.CircunferenciaCintura ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaMuslo", (object?)medicion.CircunferenciaMuslo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Notas", (object?)medicion.Notas ?? DBNull.Value);

                    var result = await command.ExecuteScalarAsync();
                    medicion.MedicionID = Convert.ToInt32(result);
                    return medicion.MedicionID;
                }
            }
        }

        public async Task UpdateAsync(Medicion medicion)
        {
            // Calcular IMC si se actualizaron peso y altura
            if (medicion.Peso.HasValue && medicion.Altura.HasValue && medicion.Altura.Value > 0)
            {
                // IMC = peso(kg) / (altura(m))²
                medicion.IMC = medicion.Peso.Value / (medicion.Altura.Value * medicion.Altura.Value);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"UPDATE Mediciones
                                SET Fecha = @Fecha, 
                                    Peso = @Peso, 
                                    Altura = @Altura, 
                                    IMC = @IMC, 
                                    PorcentajeGrasa = @PorcentajeGrasa, 
                                    CircunferenciaBrazo = @CircunferenciaBrazo, 
                                    CircunferenciaPecho = @CircunferenciaPecho, 
                                    CircunferenciaCintura = @CircunferenciaCintura, 
                                    CircunferenciaMuslo = @CircunferenciaMuslo, 
                                    Notas = @Notas
                                WHERE MedicionID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", medicion.MedicionID);
                    command.Parameters.AddWithValue("@Fecha", medicion.Fecha);
                    command.Parameters.AddWithValue("@Peso", (object?)medicion.Peso ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Altura", (object?)medicion.Altura ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IMC", (object?)medicion.IMC ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PorcentajeGrasa", (object?)medicion.PorcentajeGrasa ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaBrazo", (object?)medicion.CircunferenciaBrazo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaPecho", (object?)medicion.CircunferenciaPecho ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaCintura", (object?)medicion.CircunferenciaCintura ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaMuslo", (object?)medicion.CircunferenciaMuslo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Notas", (object?)medicion.Notas ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM Mediciones WHERE MedicionID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}