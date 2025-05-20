// GymAPI/Service/LogroService.cs
using GymAPI.Models;
using GymAPI.Repositories;
using GymAPI.DTOs;

namespace GymAPI.Services
{
    public class LogroService : ILogroService
    {
        private readonly ILogroRepository _logroRepository;
        private readonly IUsuarioLogroRepository _usuarioLogroRepository;
        private readonly IRutinaCompletadaService _rutinaService;
        
        public LogroService(
            ILogroRepository logroRepository, 
            IUsuarioLogroRepository usuarioLogroRepository,
            IRutinaCompletadaService rutinaService)
        {
            _logroRepository = logroRepository;
            _usuarioLogroRepository = usuarioLogroRepository;
            _rutinaService = rutinaService;
        }

        public async Task<List<Logro>> GetAllLogrosAsync()
        {
            return await _logroRepository.GetAllAsync();
        }

        public async Task<Logro?> GetLogroByIdAsync(int id)
        {
            return await _logroRepository.GetByIdAsync(id);
        }

        public async Task<List<UsuarioLogroDTO>> GetUsuarioLogrosAsync(int usuarioId)
        {
            var usuarioLogros = await _usuarioLogroRepository.GetByUsuarioIdAsync(usuarioId);
            
            return usuarioLogros.Select(ul => new UsuarioLogroDTO
            {
                LogroID = ul.LogroID,
                Nombre = ul.Logro?.Nombre ?? "",
                Descripcion = ul.Logro?.Descripcion ?? "",
                Icono = ul.Logro?.Icono ?? "",
                Color = ul.Logro?.Color ?? "",
                Experiencia = ul.Logro?.Experiencia ?? 0,
                Categoria = ul.Logro?.Categoria ?? "",
                Desbloqueado = ul.Desbloqueado,
                FechaDesbloqueo = ul.Desbloqueado ? ul.FechaDesbloqueo : null,
                ProgresoActual = ul.ProgresoActual,
                ValorMeta = ul.Logro?.ValorMeta ?? 0
            }).ToList();
        }

        public async Task<List<LogroDTO>> GetLogrosDisponiblesAsync(int usuarioId)
        {
            // Obtener todos los logros
            var todosLosLogros = await _logroRepository.GetAllAsync();
            
            // Obtener los logros del usuario
            var usuarioLogros = await _usuarioLogroRepository.GetByUsuarioIdAsync(usuarioId);
            
            // Excluir los logros secretos que el usuario no ha desbloqueado
            var logrosDTO = todosLosLogros
                .Where(l => !l.Secreto || usuarioLogros.Any(ul => ul.LogroID == l.LogroID))
                .Select(l => new LogroDTO
                {
                    LogroID = l.LogroID,
                    Nombre = l.Nombre,
                    Descripcion = l.Descripcion,
                    Icono = l.Icono,
                    Color = l.Color,
                    Experiencia = l.Experiencia,
                    Categoria = l.Categoria,
                    ValorMeta = l.ValorMeta,
                    Secreto = l.Secreto
                }).ToList();
            
            return logrosDTO;
        }

        public async Task<UsuarioLogroDTO?> GetUsuarioLogroByIdAsync(int usuarioId, int logroId)
        {
            var usuarioLogro = await _usuarioLogroRepository.GetByUsuarioAndLogroIdAsync(usuarioId, logroId);
            
            if (usuarioLogro == null)
                return null;
            
            return new UsuarioLogroDTO
            {
                LogroID = usuarioLogro.LogroID,
                Nombre = usuarioLogro.Logro?.Nombre ?? "",
                Descripcion = usuarioLogro.Logro?.Descripcion ?? "",
                Icono = usuarioLogro.Logro?.Icono ?? "",
                Color = usuarioLogro.Logro?.Color ?? "",
                Experiencia = usuarioLogro.Logro?.Experiencia ?? 0,
                Categoria = usuarioLogro.Logro?.Categoria ?? "",
                Desbloqueado = usuarioLogro.Desbloqueado,
                FechaDesbloqueo = usuarioLogro.Desbloqueado ? usuarioLogro.FechaDesbloqueo : null,
                ProgresoActual = usuarioLogro.ProgresoActual,
                ValorMeta = usuarioLogro.Logro?.ValorMeta ?? 0
            };
        }

        public async Task ActualizarProgresoLogroAsync(int usuarioId, int logroId, int progreso)
        {
            var logro = await _logroRepository.GetByIdAsync(logroId);
            if (logro == null)
                return;
            
            bool desbloqueado = progreso >= logro.ValorMeta;
            
            await _usuarioLogroRepository.UpdateProgresoAsync(usuarioId, logroId, progreso, desbloqueado);
        }

        public async Task VerificarLogrosAsync(int usuarioId)
        {
            // Obtener los logros disponibles
            var todosLosLogros = await _logroRepository.GetAllAsync();
            
            // Obtener estadísticas del usuario
            var estadisticas = await _rutinaService.GetEstadisticasUsuarioAsync(usuarioId);
            
            foreach (var logro in todosLosLogros)
            {
                // Verificar cada tipo de logro según su condición
                switch (logro.CondicionLogro)
                {
                    case "TOTAL_ENTRENAMIENTOS":
                        await ActualizarProgresoLogroAsync(usuarioId, logro.LogroID, estadisticas.TotalRutinasCompletadas);
                        break;
                    
                    case "ENTRENAMIENTOS_SEMANA":
                        await ActualizarProgresoLogroAsync(usuarioId, logro.LogroID, estadisticas.RutinasUltimaSemana);
                        break;
                    
                    case "DIAS_CONSECUTIVOS":
                        // Lógica para calcular días consecutivos
                        int diasConsecutivos = CalcularDiasConsecutivos(estadisticas.DiasEntrenados);
                        await ActualizarProgresoLogroAsync(usuarioId, logro.LogroID, diasConsecutivos);
                        break;
                    
                    case "NIVEL_ROL":
                        // Obtener el nivel actual del usuario (necesitaría acceso al servicio de roles)
                        // Por ahora, simulamos con un valor constante
                        await ActualizarProgresoLogroAsync(usuarioId, logro.LogroID, 3); // Ejemplo: nivel 3
                        break;
                    
                    // Otros tipos de condiciones...
                }
            }
        }

        public async Task<List<UsuarioLogroDTO>> GetLogrosRecientesAsync(int usuarioId, int cantidad = 5)
        {
            var usuarioLogros = await _usuarioLogroRepository.GetByUsuarioIdAsync(usuarioId);
            
            return usuarioLogros
                .Where(ul => ul.Desbloqueado)
                .OrderByDescending(ul => ul.FechaDesbloqueo)
                .Take(cantidad)
                .Select(ul => new UsuarioLogroDTO
                {
                    LogroID = ul.LogroID,
                    Nombre = ul.Logro?.Nombre ?? "",
                    Descripcion = ul.Logro?.Descripcion ?? "",
                    Icono = ul.Logro?.Icono ?? "",
                    Color = ul.Logro?.Color ?? "",
                    Experiencia = ul.Logro?.Experiencia ?? 0,
                    Categoria = ul.Logro?.Categoria ?? "",
                    Desbloqueado = ul.Desbloqueado,
                    FechaDesbloqueo = ul.FechaDesbloqueo,
                    ProgresoActual = ul.ProgresoActual,
                    ValorMeta = ul.Logro?.ValorMeta ?? 0
                }).ToList();
        }

        // Método auxiliar para calcular días consecutivos
        private int CalcularDiasConsecutivos(List<FechaEntrenoDTO> diasEntrenados)
        {
            if (diasEntrenados == null || !diasEntrenados.Any())
                return 0;
            
            // Ordenar por fecha (más reciente primero)
            var diasOrdenados = diasEntrenados
                .Select(d => d.Fecha.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();
            
            if (!diasOrdenados.Any())
                return 0;
            
            int maxConsecutivos = 1;
            int consecutivosActuales = 1;
            var fechaAnterior = diasOrdenados[0];
            
            for (int i = 1; i < diasOrdenados.Count; i++)
            {
                var fechaActual = diasOrdenados[i];
                
                // Si es justo el día anterior, incrementar racha
                if ((fechaAnterior - fechaActual).Days == 1)
                {
                    consecutivosActuales++;
                    maxConsecutivos = Math.Max(maxConsecutivos, consecutivosActuales);
                }
                else if ((fechaAnterior - fechaActual).Days > 1)
                {
                    // Si hay un hueco, reiniciar contador
                    consecutivosActuales = 1;
                }
                
                fechaAnterior = fechaActual;
            }
            
            return maxConsecutivos;
        }
    }
}