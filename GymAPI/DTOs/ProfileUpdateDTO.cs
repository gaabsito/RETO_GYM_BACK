namespace GymAPI.DTOs
{
    public class ProfileUpdateDTO
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }

        public int? Edad { get; set; }

        public float? Peso { get; set; }

        public float? Altura { get; set; }

        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}