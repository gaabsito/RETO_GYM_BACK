namespace GymAPI.Models
{
    public class RutinaCompletada
    {
        public int RutinaCompletadaID { get; set; }
        public int UsuarioID { get; set; }
        public int EntrenamientoID { get; set; }
        public DateTime FechaCompletada { get; set; } = DateTime.Now;
        public string? Notas { get; set; }
        public int? DuracionMinutos { get; set; }
        public int? CaloriasEstimadas { get; set; }
        public int? NivelEsfuerzoPercibido { get; set; } // Escala de 1-10
        
        // Referencias de navegación
        public Usuario? Usuario { get; set; }
        public Entrenamiento? Entrenamiento { get; set; }
    }
}