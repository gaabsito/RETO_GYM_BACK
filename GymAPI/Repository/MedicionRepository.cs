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
                            Peso = ConvertToNullableSingle(reader, 3),
                            Altura = ConvertToNullableSingle(reader, 4),
                            IMC = ConvertToNullableSingle(reader, 5),
                            PorcentajeGrasa = ConvertToNullableSingle(reader, 6),
                            CircunferenciaBrazo = ConvertToNullableSingle(reader, 7),
                            CircunferenciaPecho = ConvertToNullableSingle(reader, 8),
                            CircunferenciaCintura = ConvertToNullableSingle(reader, 9),
                            CircunferenciaMuslo = ConvertToNullableSingle(reader, 10),
                            Notas = reader.IsDBNull(11) ? null : reader.GetString(11)
                        });
                    }
                }
            }
            return mediciones;
        }

        public async Task<Medicion?> GetByIdAsync(int id)
        {
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
                            return new Medicion
                            {
                                MedicionID = reader.GetInt32(0),
                                UsuarioID = reader.GetInt32(1),
                                Fecha = reader.GetDateTime(2),
                                Peso = ConvertToNullableSingle(reader, 3),
                                Altura = ConvertToNullableSingle(reader, 4),
                                IMC = ConvertToNullableSingle(reader, 5),
                                PorcentajeGrasa = ConvertToNullableSingle(reader, 6),
                                CircunferenciaBrazo = ConvertToNullableSingle(reader, 7),
                                CircunferenciaPecho = ConvertToNullableSingle(reader, 8),
                                CircunferenciaCintura = ConvertToNullableSingle(reader, 9),
                                CircunferenciaMuslo = ConvertToNullableSingle(reader, 10),
                                Notas = reader.IsDBNull(11) ? null : reader.GetString(11)
                            };
                        }
                    }
                }
            }
            return null;
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
                                Peso = ConvertToNullableSingle(reader, 3),
                                Altura = ConvertToNullableSingle(reader, 4),
                                IMC = ConvertToNullableSingle(reader, 5),
                                PorcentajeGrasa = ConvertToNullableSingle(reader, 6),
                                CircunferenciaBrazo = ConvertToNullableSingle(reader, 7),
                                CircunferenciaPecho = ConvertToNullableSingle(reader, 8),
                                CircunferenciaCintura = ConvertToNullableSingle(reader, 9),
                                CircunferenciaMuslo = ConvertToNullableSingle(reader, 10),
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
                                PesoPromedio = ConvertToNullableSingle(reader, 3),
                                IMCPromedio = ConvertToNullableSingle(reader, 4),
                                GrasaPromedio = ConvertToNullableSingle(reader, 5),
                                CinturaPromedio = ConvertToNullableSingle(reader, 6)
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
                    command.Parameters.AddWithValue("@Peso", medicion.Peso ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Altura", medicion.Altura ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@IMC", medicion.IMC ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PorcentajeGrasa", medicion.PorcentajeGrasa ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaBrazo", medicion.CircunferenciaBrazo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaPecho", medicion.CircunferenciaPecho ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaCintura", medicion.CircunferenciaCintura ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaMuslo", medicion.CircunferenciaMuslo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Notas", medicion.Notas ?? (object)DBNull.Value);

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
                    command.Parameters.AddWithValue("@Peso", medicion.Peso ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Altura", medicion.Altura ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@IMC", medicion.IMC ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PorcentajeGrasa", medicion.PorcentajeGrasa ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaBrazo", medicion.CircunferenciaBrazo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaPecho", medicion.CircunferenciaPecho ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaCintura", medicion.CircunferenciaCintura ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CircunferenciaMuslo", medicion.CircunferenciaMuslo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Notas", medicion.Notas ?? (object)DBNull.Value);

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

        private float? ConvertToNullableSingle(SqlDataReader reader, int columnIndex)
        {
            try
            {
                if (reader.IsDBNull(columnIndex)) return null;

                object value = reader.GetValue(columnIndex);

                return value switch
                {
                    double doubleValue => (float)doubleValue,
                    float floatValue => floatValue,
                    decimal decimalValue => (float)decimalValue,
                    _ => Convert.ToSingle(value)
                };
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}