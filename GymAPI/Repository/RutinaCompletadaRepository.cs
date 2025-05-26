using Microsoft.Data.SqlClient;
using GymAPI.Models;

namespace GymAPI.Repositories
{
    public class RutinaCompletadaRepository : IRutinaCompletadaRepository
    {
        private readonly string _connectionString;

        public RutinaCompletadaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<RutinaCompletada>> GetAllAsync()
        {
            var rutinasCompletadas = new List<RutinaCompletada>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT RutinaCompletadaID, UsuarioID, EntrenamientoID, 
                               FechaCompletada, Notas, DuracionMinutos, 
                               CaloriasEstimadas, NivelEsfuerzoPercibido FROM RutinasCompletadas";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        rutinasCompletadas.Add(new RutinaCompletada
                        {
                            RutinaCompletadaID = reader.GetInt32(0),
                            UsuarioID = reader.GetInt32(1),
                            EntrenamientoID = reader.GetInt32(2),
                            FechaCompletada = reader.GetDateTime(3),
                            Notas = reader.IsDBNull(4) ? null : reader.GetString(4),
                            DuracionMinutos = reader.IsDBNull(5) ? null : (int?)reader.GetInt32(5),
                            CaloriasEstimadas = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6),
                            NivelEsfuerzoPercibido = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7)
                        });
                    }
                }
            }
            return rutinasCompletadas;
        }

        public async Task<RutinaCompletada?> GetByIdAsync(int id)
        {
            RutinaCompletada? rutinaCompletada = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT RutinaCompletadaID, UsuarioID, EntrenamientoID, 
                               FechaCompletada, Notas, DuracionMinutos, 
                               CaloriasEstimadas, NivelEsfuerzoPercibido 
                               FROM RutinasCompletadas 
                               WHERE RutinaCompletadaID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            rutinaCompletada = new RutinaCompletada
                            {
                                RutinaCompletadaID = reader.GetInt32(0),
                                UsuarioID = reader.GetInt32(1),
                                EntrenamientoID = reader.GetInt32(2),
                                FechaCompletada = reader.GetDateTime(3),
                                Notas = reader.IsDBNull(4) ? null : reader.GetString(4),
                                DuracionMinutos = reader.IsDBNull(5) ? null : (int?)reader.GetInt32(5),
                                CaloriasEstimadas = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6),
                                NivelEsfuerzoPercibido = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7)
                            };
                        }
                    }
                }
            }
            return rutinaCompletada;
        }

        public async Task<List<RutinaCompletada>> GetByUsuarioIdAsync(int usuarioId)
        {
            var rutinasCompletadas = new List<RutinaCompletada>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT rc.RutinaCompletadaID, rc.UsuarioID, rc.EntrenamientoID, 
                                rc.FechaCompletada, rc.Notas, rc.DuracionMinutos, 
                                rc.CaloriasEstimadas, rc.NivelEsfuerzoPercibido,
                                e.Titulo as NombreEntrenamiento, e.Dificultad
                                FROM RutinasCompletadas rc
                                JOIN Entrenamientos e ON rc.EntrenamientoID = e.EntrenamientoID
                                WHERE rc.UsuarioID = @UsuarioId
                                ORDER BY rc.FechaCompletada DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var rutinaCompletada = new RutinaCompletada
                            {
                                RutinaCompletadaID = reader.GetInt32(0),
                                UsuarioID = reader.GetInt32(1),
                                EntrenamientoID = reader.GetInt32(2),
                                FechaCompletada = reader.GetDateTime(3),
                                Notas = reader.IsDBNull(4) ? null : reader.GetString(4),
                                DuracionMinutos = reader.IsDBNull(5) ? null : (int?)reader.GetInt32(5),
                                CaloriasEstimadas = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6),
                                NivelEsfuerzoPercibido = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7),
                                Entrenamiento = new Entrenamiento
                                {
                                    Titulo = reader.GetString(8),
                                    Dificultad = reader.GetString(9)
                                }
                            };
                            
                            rutinasCompletadas.Add(rutinaCompletada);
                        }
                    }
                }
            }

            return rutinasCompletadas;
        }

        public async Task<List<RutinaCompletada>> GetByEntrenamientoIdAsync(int entrenamientoId)
        {
            var rutinasCompletadas = new List<RutinaCompletada>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT RutinaCompletadaID, UsuarioID, EntrenamientoID, 
                                FechaCompletada, Notas, DuracionMinutos, 
                                CaloriasEstimadas, NivelEsfuerzoPercibido
                                FROM RutinasCompletadas
                                WHERE EntrenamientoID = @EntrenamientoId
                                ORDER BY FechaCompletada DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EntrenamientoId", entrenamientoId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            rutinasCompletadas.Add(new RutinaCompletada
                            {
                                RutinaCompletadaID = reader.GetInt32(0),
                                UsuarioID = reader.GetInt32(1),
                                EntrenamientoID = reader.GetInt32(2),
                                FechaCompletada = reader.GetDateTime(3),
                                Notas = reader.IsDBNull(4) ? null : reader.GetString(4),
                                DuracionMinutos = reader.IsDBNull(5) ? null : (int?)reader.GetInt32(5),
                                CaloriasEstimadas = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6),
                                NivelEsfuerzoPercibido = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7)
                            });
                        }
                    }
                }
            }

            return rutinasCompletadas;
        }

        public async Task<List<RutinaCompletada>> GetByUsuarioIdAndEntrenamientoIdAsync(int usuarioId, int entrenamientoId)
        {
            var rutinasCompletadas = new List<RutinaCompletada>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT RutinaCompletadaID, UsuarioID, EntrenamientoID, 
                                FechaCompletada, Notas, DuracionMinutos, 
                                CaloriasEstimadas, NivelEsfuerzoPercibido
                                FROM RutinasCompletadas
                                WHERE UsuarioID = @UsuarioId AND EntrenamientoID = @EntrenamientoId
                                ORDER BY FechaCompletada DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    command.Parameters.AddWithValue("@EntrenamientoId", entrenamientoId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            rutinasCompletadas.Add(new RutinaCompletada
                            {
                                RutinaCompletadaID = reader.GetInt32(0),
                                UsuarioID = reader.GetInt32(1),
                                EntrenamientoID = reader.GetInt32(2),
                                FechaCompletada = reader.GetDateTime(3),
                                Notas = reader.IsDBNull(4) ? null : reader.GetString(4),
                                DuracionMinutos = reader.IsDBNull(5) ? null : (int?)reader.GetInt32(5),
                                CaloriasEstimadas = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6),
                                NivelEsfuerzoPercibido = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7)
                            });
                        }
                    }
                }
            }

            return rutinasCompletadas;
        }

        public async Task<int> AddAsync(RutinaCompletada rutinaCompletada)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO RutinasCompletadas (UsuarioID, EntrenamientoID, FechaCompletada, 
                                Notas, DuracionMinutos, CaloriasEstimadas, NivelEsfuerzoPercibido)
                                VALUES (@UsuarioID, @EntrenamientoID, @FechaCompletada, 
                                @Notas, @DuracionMinutos, @CaloriasEstimadas, @NivelEsfuerzoPercibido);
                                SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioID", rutinaCompletada.UsuarioID);
                    command.Parameters.AddWithValue("@EntrenamientoID", rutinaCompletada.EntrenamientoID);
                    command.Parameters.AddWithValue("@FechaCompletada", rutinaCompletada.FechaCompletada);
                    command.Parameters.AddWithValue("@Notas", rutinaCompletada.Notas ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DuracionMinutos", rutinaCompletada.DuracionMinutos ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CaloriasEstimadas", rutinaCompletada.CaloriasEstimadas ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@NivelEsfuerzoPercibido", rutinaCompletada.NivelEsfuerzoPercibido ?? (object)DBNull.Value);

                    var result = await command.ExecuteScalarAsync();
                    rutinaCompletada.RutinaCompletadaID = Convert.ToInt32(result);
                    return rutinaCompletada.RutinaCompletadaID;
                }
            }
        }

        public async Task UpdateAsync(RutinaCompletada rutinaCompletada)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"UPDATE RutinasCompletadas
                                SET FechaCompletada = @FechaCompletada,
                                    Notas = @Notas,
                                    DuracionMinutos = @DuracionMinutos,
                                    CaloriasEstimadas = @CaloriasEstimadas,
                                    NivelEsfuerzoPercibido = @NivelEsfuerzoPercibido
                                WHERE RutinaCompletadaID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", rutinaCompletada.RutinaCompletadaID);
                    command.Parameters.AddWithValue("@FechaCompletada", rutinaCompletada.FechaCompletada);
                    command.Parameters.AddWithValue("@Notas", rutinaCompletada.Notas ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DuracionMinutos", rutinaCompletada.DuracionMinutos ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CaloriasEstimadas", rutinaCompletada.CaloriasEstimadas ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@NivelEsfuerzoPercibido", rutinaCompletada.NivelEsfuerzoPercibido ?? (object)DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM RutinasCompletadas WHERE RutinaCompletadaID = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> GetCountLastWeekAsync(int usuarioId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT COUNT(*) 
                                FROM RutinasCompletadas 
                                WHERE UsuarioID = @UsuarioId 
                                AND FechaCompletada >= DATEADD(day, -7, GETDATE())";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<int> GetCountLastMonthAsync(int usuarioId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT COUNT(*) 
                                FROM RutinasCompletadas 
                                WHERE UsuarioID = @UsuarioId 
                                AND FechaCompletada >= DATEADD(day, -30, GETDATE())";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<int> GetTotalCountAsync(int usuarioId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT COUNT(*) 
                                FROM RutinasCompletadas 
                                WHERE UsuarioID = @UsuarioId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<(int EntrenamientoID, string Nombre, int Veces)> GetMostCompletedWorkoutAsync(int usuarioId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT TOP 1 rc.EntrenamientoID, e.Titulo, COUNT(*) as Veces
                                FROM RutinasCompletadas rc
                                JOIN Entrenamientos e ON rc.EntrenamientoID = e.EntrenamientoID
                                WHERE rc.UsuarioID = @UsuarioId
                                GROUP BY rc.EntrenamientoID, e.Titulo
                                ORDER BY COUNT(*) DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return (
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetInt32(2)
                            );
                        }
                        return (0, string.Empty, 0);
                    }
                }
            }
        }

        // MÉTODO PARA BUSCAR POR PERÍODO
        public async Task<List<RutinaCompletada>> GetByUsuarioIdAndPeriodAsync(int usuarioId, int month, int year)
        {
            var rutinasCompletadas = new List<RutinaCompletada>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT rc.RutinaCompletadaID, rc.UsuarioID, rc.EntrenamientoID, 
                                rc.FechaCompletada, rc.Notas, rc.DuracionMinutos, 
                                rc.CaloriasEstimadas, rc.NivelEsfuerzoPercibido,
                                e.Titulo as NombreEntrenamiento, e.Dificultad
                                FROM RutinasCompletadas rc
                                LEFT JOIN Entrenamientos e ON rc.EntrenamientoID = e.EntrenamientoID
                                WHERE rc.UsuarioID = @UsuarioId
                                AND MONTH(rc.FechaCompletada) = @Month
                                AND YEAR(rc.FechaCompletada) = @Year
                                ORDER BY rc.FechaCompletada DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    command.Parameters.AddWithValue("@Month", month);
                    command.Parameters.AddWithValue("@Year", year);
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var rutinaCompletada = new RutinaCompletada
                            {
                                RutinaCompletadaID = reader.GetInt32(0),
                                UsuarioID = reader.GetInt32(1),
                                EntrenamientoID = reader.GetInt32(2),
                                FechaCompletada = reader.GetDateTime(3),
                                Notas = reader.IsDBNull(4) ? null : reader.GetString(4),
                                DuracionMinutos = reader.IsDBNull(5) ? null : (int?)reader.GetInt32(5),
                                CaloriasEstimadas = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6),
                                NivelEsfuerzoPercibido = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7)
                            };

                            // Solo agregar información del entrenamiento si no es NULL
                            if (!reader.IsDBNull(8))
                            {
                                rutinaCompletada.Entrenamiento = new Entrenamiento
                                {
                                    Titulo = reader.GetString(8),
                                    Dificultad = reader.IsDBNull(9) ? "" : reader.GetString(9)
                                };
                            }
                            
                            rutinasCompletadas.Add(rutinaCompletada);
                        }
                    }
                }
            }

            return rutinasCompletadas;
        }

        // NUEVO MÉTODO PARA CONTAR DÍAS ÚNICOS ENTRENADOS EN LA SEMANA ACTUAL
        public async Task<int> GetUniqueTrainingDaysThisWeekAsync(int usuarioId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Esta consulta cuenta los días DISTINTOS en la semana actual
                // usando DATEPART(ww, GETDATE()) para obtener el número de semana del año actual
                string query = @"
                    SELECT COUNT(DISTINCT CAST(FechaCompletada AS DATE)) 
                    FROM RutinasCompletadas 
                    WHERE UsuarioID = @UsuarioId 
                      AND DATEPART(ww, FechaCompletada) = DATEPART(ww, GETDATE())
                      AND DATEPART(yyyy, FechaCompletada) = DATEPART(yyyy, GETDATE())";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        // MÉTODO PARA OBTENER ESTADÍSTICAS POR SEMANA
        public async Task<Dictionary<int, int>> GetUniqueTrainingDaysLastWeeksAsync(int usuarioId, int numberOfWeeks)
        {
            var trainingDaysByWeek = new Dictionary<int, int>();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Esta consulta obtiene el conteo de días únicos por semana para las últimas N semanas
                string query = @"
                    WITH WeeklyTrainingData AS (
                        SELECT 
                            DATEPART(ww, FechaCompletada) AS WeekNumber,
                            DATEPART(yyyy, FechaCompletada) AS Year,
                            CAST(FechaCompletada AS DATE) AS TrainingDate
                        FROM RutinasCompletadas 
                        WHERE UsuarioID = @UsuarioId 
                          AND FechaCompletada >= DATEADD(WEEK, -@NumberOfWeeks, GETDATE())
                    )
                    SELECT WeekNumber, Year, COUNT(DISTINCT TrainingDate) AS UniqueTrainingDays
                    FROM WeeklyTrainingData
                    GROUP BY WeekNumber, Year
                    ORDER BY Year DESC, WeekNumber DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    command.Parameters.AddWithValue("@NumberOfWeeks", numberOfWeeks);
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int weekNumber = reader.GetInt32(0);
                            int year = reader.GetInt32(1);
                            int trainingDays = reader.GetInt32(2);
                            
                            // Usamos una clave compuesta (año-semana) para identificar cada semana de forma única
                            int weekKey = year * 100 + weekNumber;
                            trainingDaysByWeek.Add(weekKey, trainingDays);
                        }
                    }
                }
            }
            
            return trainingDaysByWeek;
        }
    }
}