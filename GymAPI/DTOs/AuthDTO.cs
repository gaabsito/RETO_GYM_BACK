namespace GymAPI.DTOs
{
    public class RegisterDTO
    {
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class AuthResponseDTO
    {
        public UsuarioDTO User { get; set; } = null!;
        public string Token { get; set; } = "";
        public bool EsAdministrador { get; set; } = false; // NUEVO
    }

    public class AdminCreateUserDTO
    {
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public bool EsAdmin { get; set; } = false;
        public bool EstaActivo { get; set; } = true;
    }
}