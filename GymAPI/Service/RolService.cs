using GymAPI.Models;
using GymAPI.Services;

namespace GymAPI.Services
{
    public class RolService : IRolService
    {
        private readonly List<Rol> _roles;
        private readonly IRutinaCompletadaService _rutinaService;

        public RolService(IRutinaCompletadaService rutinaService)
        {
            _rutinaService = rutinaService;
            
            // Roles predefinidos
            _roles = new List<Rol>
            {
                new Rol { 
                    RolID = 1, 
                    Nombre = "Principiante", 
                    Descripcion = "Estás empezando tu camino fitness. ¡El primer paso es el más importante!", 
                    Icono = "mdi-run", 
                    Color = "#9E9E9E", 
                    DiasPorSemanaMinimo = 0, 
                    DiasPorSemanaMaximo = 1 
                },
                new Rol { 
                    RolID = 2, 
                    Nombre = "Constante", 
                    Descripcion = "Empiezas a crear un hábito saludable entrenando regularmente.", 
                    Icono = "mdi-trending-up", 
                    Color = "#8BC34A", 
                    DiasPorSemanaMinimo = 2, 
                    DiasPorSemanaMaximo = 2 
                },
                new Rol { 
                    RolID = 3, 
                    Nombre = "Comprometido", 
                    Descripcion = "Tu compromiso con el entrenamiento es evidente. ¡Sigue así!", 
                    Icono = "mdi-arm-flex", 
                    Color = "#4CAF50", 
                    DiasPorSemanaMinimo = 3, 
                    DiasPorSemanaMaximo = 3 
                },
                new Rol { 
                    RolID = 4, 
                    Nombre = "Dedicado", 
                    Descripcion = "Entrenas más de la mitad de la semana. ¡Tu dedicación es admirable!", 
                    Icono = "mdi-weight-lifter", 
                    Color = "#2196F3", 
                    DiasPorSemanaMinimo = 4, 
                    DiasPorSemanaMaximo = 4 
                },
                new Rol { 
                    RolID = 5, 
                    Nombre = "Disciplinado", 
                    Descripcion = "5 días a la semana. ¡Tu disciplina está construyendo resultados increíbles!", 
                    Icono = "mdi-medal", 
                    Color = "#FF9800", 
                    DiasPorSemanaMinimo = 5, 
                    DiasPorSemanaMaximo = 5 
                },
                new Rol { 
                    RolID = 6, 
                    Nombre = "Atleta", 
                    Descripcion = "Entrenas casi todos los días. ¡Eres un verdadero atleta!", 
                    Icono = "mdi-trophy", 
                    Color = "#F44336", 
                    DiasPorSemanaMinimo = 6, 
                    DiasPorSemanaMaximo = 6 
                },
                new Rol { 
                    RolID = 7, 
                    Nombre = "Élite", 
                    Descripcion = "¡Entrenas todos los días! Tu dedicación te coloca en la élite fitness.", 
                    Icono = "mdi-crown", 
                    Color = "#E91E63", 
                    DiasPorSemanaMinimo = 7, 
                    DiasPorSemanaMaximo = 7 
                }
            };
        }

        public Task<List<Rol>> GetAllRolesAsync()
        {
            return Task.FromResult(_roles);
        }

        public Task<Rol?> GetRolByIdAsync(int id)
        {
            var rol = _roles.FirstOrDefault(r => r.RolID == id);
            return Task.FromResult(rol);
        }

        public async Task<Rol?> GetUserCurrentRolAsync(int userId)
        {
            // Obtener días ÚNICOS entrenados en la semana actual
            int diasEntrenadosUnicos = await _rutinaService.GetUniqueTrainingDaysThisWeekAsync(userId);
            
            // Encontrar el rol correspondiente
            Rol? rolActual = null;
            
            foreach (var rol in _roles.OrderBy(r => r.RolID))
            {
                if (diasEntrenadosUnicos >= rol.DiasPorSemanaMinimo && 
                    diasEntrenadosUnicos <= rol.DiasPorSemanaMaximo)
                {
                    rolActual = rol;
                    break;
                }
            }
            
            // Si no coincide con ningún rango específico pero entrena 7 días, asignar rol Élite
            if (rolActual == null && diasEntrenadosUnicos >= 7)
            {
                rolActual = _roles.FirstOrDefault(r => r.RolID == 7);
            }
            
            // Si no coincide con ningún criterio, usar el rol Principiante por defecto
            return rolActual ?? _roles.FirstOrDefault(r => r.RolID == 1);
        }

        public async Task<(Rol? CurrentRol, int DiasEntrenados, int DiasParaSiguiente, double Progreso)> GetUserRolInfoAsync(int userId)
        {
            // Obtener días ÚNICOS entrenados en la semana actual
            int diasEntrenadosUnicos = await _rutinaService.GetUniqueTrainingDaysThisWeekAsync(userId);
            
            // Obtener rol actual
            var rolActual = await GetUserCurrentRolAsync(userId);
            if (rolActual == null)
                return (null, diasEntrenadosUnicos, 0, 0);
            
            // Si ya es el rol máximo, no hay siguiente rol
            if (rolActual.RolID == 7)
                return (rolActual, diasEntrenadosUnicos, 0, 100);
            
            // Calcular días para el siguiente rol
            var siguienteRol = _roles.FirstOrDefault(r => r.RolID == rolActual.RolID + 1);
            if (siguienteRol == null)
                return (rolActual, diasEntrenadosUnicos, 0, 100);
            
            int diasParaSiguiente = Math.Max(0, siguienteRol.DiasPorSemanaMinimo - diasEntrenadosUnicos);
            
            // Calcular progreso hacia el siguiente rol
            double progreso = 0;
            if (rolActual.DiasPorSemanaMaximo == rolActual.DiasPorSemanaMinimo)
            {
                // Si el rol actual tiene un rango único (ej: 2-2 días)
                progreso = diasEntrenadosUnicos >= rolActual.DiasPorSemanaMaximo ? 100 : 0;
            }
            else
            {
                // Calcular progreso dentro del rango
                int rangoTotal = rolActual.DiasPorSemanaMaximo - rolActual.DiasPorSemanaMinimo;
                int progresoActual = diasEntrenadosUnicos - rolActual.DiasPorSemanaMinimo;
                progreso = Math.Min(100, Math.Max(0, (double)progresoActual / rangoTotal * 100));
            }
            
            return (rolActual, diasEntrenadosUnicos, diasParaSiguiente, progreso);
        }
    }
}