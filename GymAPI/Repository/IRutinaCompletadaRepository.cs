using GymAPI.Models;

namespace GymAPI.Repositories
{
    public interface IRutinaCompletadaRepository
    {
        Task<List<RutinaCompletada>> GetAllAsync();
        Task<RutinaCompletada?> GetByIdAsync(int id);
        Task<List<RutinaCompletada>> GetByUsuarioIdAsync(int usuarioId);
        Task<List<RutinaCompletada>> GetByEntrenamientoIdAsync(int entrenamientoId);
        Task<List<RutinaCompletada>> GetByUsuarioIdAndEntrenamientoIdAsync(int usuarioId, int entrenamientoId);
        Task<int> AddAsync(RutinaCompletada rutinaCompletada);
        Task UpdateAsync(RutinaCompletada rutinaCompletada);
        Task DeleteAsync(int id);
        
        // Métodos para estadísticas
        Task<int> GetCountLastWeekAsync(int usuarioId);
        Task<int> GetCountLastMonthAsync(int usuarioId);
        Task<int> GetTotalCountAsync(int usuarioId);
        Task<(int EntrenamientoID, string Nombre, int Veces)> GetMostCompletedWorkoutAsync(int usuarioId);
        
        // Nuevo método para buscar por período
        Task<List<RutinaCompletada>> GetByUsuarioIdAndPeriodAsync(int usuarioId, int month, int year);
        
        // Nuevo método para contar días únicos entrenados esta semana
        Task<int> GetUniqueTrainingDaysThisWeekAsync(int usuarioId);
        
        // Método adicional para obtener estadísticas por semanas
        Task<Dictionary<int, int>> GetUniqueTrainingDaysLastWeeksAsync(int usuarioId, int numberOfWeeks);
    }
}