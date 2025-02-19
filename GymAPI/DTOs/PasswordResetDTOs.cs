using System.ComponentModel.DataAnnotations;

namespace GymAPI.DTOs
{
    public class RequestPasswordResetDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }

    public class ResetPasswordDTO
    {
        [Required]
        public string Token { get; set; } = "";

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = "";

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = "";
    }
}