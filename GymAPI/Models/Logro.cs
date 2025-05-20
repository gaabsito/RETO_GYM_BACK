// GymAPI/Models/Logro.cs
namespace GymAPI.Models
{
    public class Logro
    {
        public int LogroID { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public string Icono { get; set; } = "";
        public string Color { get; set; } = "";
        public int Experiencia { get; set; } // Puntos XP que otorga
        public string Categoria { get; set; } = ""; // Entrenamiento, Consistencia, Social, etc.
        public string CondicionLogro { get; set; } = ""; // Código de la condición para obtener el logro
        public int ValorMeta { get; set; } // Valor objetivo para desbloquear el logro
        public bool Secreto { get; set; } = false; // Si es true, no se muestra hasta desbloquearlo
    }
}