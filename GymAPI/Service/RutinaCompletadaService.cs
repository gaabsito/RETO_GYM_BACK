using GymAPI.Models;
using GymAPI.Repositories;
using GymAPI.DTOs;

namespace GymAPI.Services
{
    public class RutinaCompletadaService : IRutinaCompletadaService
    {
        private readonly IRutinaCompletadaRepository _repository;

        public RutinaCompletadaService(IRutinaCompletadaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RutinaCompletada>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<RutinaCompletada?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<RutinaCompletada>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _repository.GetByUsuarioIdAsync(usuarioId);
        }

        public async Task<List<RutinaCompletada>> GetByEntrenamientoIdAsync(int entrenamientoId)
        {
            return await _repository.GetByEntrenamientoIdAsync(entrenamientoId);
        }

        public async Task<List<RutinaCompletada>> GetByUsuarioIdAndEntrenamientoIdAsync(int usuarioId, int entrenamientoId)
        {
            return await _repository.GetByUsuarioIdAndEntrenamientoIdAsync(usuarioId, entrenamientoId);
        }

        public async Task<int> AddAsync(RutinaCompletada rutinaCompletada)
        {
            return await _repository.AddAsync(rutinaCompletada);
        }

        public async Task UpdateAsync(RutinaCompletada rutinaCompletada)
        {
            await _repository.UpdateAsync(rutinaCompletada);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<int> GetCountLastWeekAsync(int usuarioId)
        {
            return await _repository.GetCountLastWeekAsync(usuarioId);
        }

        public async Task<int> GetCountLastMonthAsync(int usuarioId)
        {
            return await _repository.GetCountLastMonthAsync(usuarioId);
        }

        public async Task<int> GetTotalCountAsync(int usuarioId)
        {
            return await _repository.GetTotalCountAsync(usuarioId);
        }

        public async Task<(int EntrenamientoID, string Nombre, int Veces)> GetMostCompletedWorkoutAsync(int usuarioId)
        {
            return await _repository.GetMostCompletedWorkoutAsync(usuarioId);
        }

        // NUEVOS MÉTODOS

        public async Task<List<RutinaCompletada>> GetByUsuarioIdAndFechaAsync(int usuarioId, int month, int year)
        {
            return await _repository.GetByUsuarioIdAndPeriodAsync(usuarioId, month, year);
        }

        public async Task<Dictionary<DateTime, int>> GetCalendarDataAsync(int usuarioId, int month, int year)
        {
            var rutinas = await _repository.GetByUsuarioIdAndPeriodAsync(usuarioId, month, year);
            
            // Agrupar por fecha para tener un conteo por día
            var calendar = rutinas
                .GroupBy(r => r.FechaCompletada.Date)
                .ToDictionary(g => g.Key, g => g.Count());
            
            return calendar;
        }

        public async Task<RutinaEstadisticasDTO> GetEstadisticasUsuarioAsync(int usuarioId)
        {
            // Obtener el número de la semana actual
            int semanaActual = System.Globalization.ISOWeek.GetWeekOfYear(DateTime.Now);
            
            var estadisticas = new RutinaEstadisticasDTO
            {
                TotalRutinasCompletadas = await GetTotalCountAsync(usuarioId),
                RutinasUltimaSemana = await GetCountLastWeekAsync(usuarioId),
                RutinasUltimoMes = await GetCountLastMonthAsync(usuarioId),
                DiasUnicosEntrenadosEstaSemana = await GetUniqueTrainingDaysThisWeekAsync(usuarioId),
                SemanaActual = semanaActual
            };
            
            // Obtener todas las rutinas completadas del usuario
            var rutinas = await GetByUsuarioIdAsync(usuarioId);
            
            // Calcular estadísticas adicionales si hay rutinas
            if (rutinas.Any())
            {
                // Promedio de esfuerzo percibido
                var esfuerzos = rutinas
                    .Where(r => r.NivelEsfuerzoPercibido.HasValue)
                    .Select(r => r.NivelEsfuerzoPercibido.Value);
                
                estadisticas.PromedioEsfuerzo = esfuerzos.Any() 
                    ? esfuerzos.Average() 
                    : 0;
                
                // Suma de calorías estimadas
                estadisticas.CaloriasTotales = rutinas
                    .Where(r => r.CaloriasEstimadas.HasValue)
                    .Select(r => r.CaloriasEstimadas.Value)
                    .Sum();
                
                // Suma de minutos de duración
                estadisticas.MinutosTotales = rutinas
                    .Where(r => r.DuracionMinutos.HasValue)
                    .Select(r => r.DuracionMinutos.Value)
                    .Sum();
                
                // Entrenamiento más repetido
                var entrenamientoMasCompletado = await GetMostCompletedWorkoutAsync(usuarioId);
                if (entrenamientoMasCompletado.EntrenamientoID != 0)
                {
                    estadisticas.EntrenamientoIDMasRepetido = entrenamientoMasCompletado.EntrenamientoID;
                    estadisticas.NombreEntrenamientoMasRepetido = entrenamientoMasCompletado.Nombre;
                    estadisticas.VecesCompletado = entrenamientoMasCompletado.Veces;
                }
                
                // Generar datos para el calendario
                estadisticas.DiasEntrenados = rutinas
                    .OrderByDescending(r => r.FechaCompletada)
                    .Select(r => new FechaEntrenoDTO
                    {
                        Fecha = r.FechaCompletada,
                        EntrenamientoID = r.EntrenamientoID,
                        NombreEntrenamiento = r.Entrenamiento?.Titulo ?? "Sin nombre"
                    })
                    .ToList();
                
                // Añadir datos de días entrenados por semana (últimas 8 semanas)
                var diasPorSemana = await GetUniqueTrainingDaysLastWeeksAsync(usuarioId, 8);
                estadisticas.DiasUnicosPorSemana = diasPorSemana;
            }
            
            return estadisticas;
        }
        
        // Nuevo método para días únicos entrenados esta semana
        public async Task<int> GetUniqueTrainingDaysThisWeekAsync(int usuarioId)
        {
            return await _repository.GetUniqueTrainingDaysThisWeekAsync(usuarioId);
        }
        
        // Método para estadísticas semanales
        public async Task<Dictionary<int, int>> GetUniqueTrainingDaysLastWeeksAsync(int usuarioId, int numberOfWeeks)
        {
            return await _repository.GetUniqueTrainingDaysLastWeeksAsync(usuarioId, numberOfWeeks);
        }
    }
}