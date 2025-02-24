using System.Text.Json.Serialization;

namespace GymAPI.Models
{
    public class Usuario
{
    public int UsuarioID { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime FechaRegistro { get; set; }
    public bool EstaActivo { get; set; }
    public string ResetPasswordToken { get; set; }
    public DateTime? ResetPasswordExpires { get; set; }

    // Campos nuevos
    public int? Edad { get; set; }
    public float? Altura { get; set; }
    public float? Peso { get; set; }
}

}
