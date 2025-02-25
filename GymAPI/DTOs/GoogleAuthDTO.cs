using System.ComponentModel.DataAnnotations;

namespace GymAPI.DTOs
{
    public class GoogleAuthDTO
    {
        [Required]
        public string? IdToken { get; set; }
    }

    public class GoogleUserInfo
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public bool EmailVerified { get; set; }
        public string? Name { get; set; }
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
        public string? Picture { get; set; }
        public string? Locale { get; set; }
    }

    public class GoogleTokenValidationResult
    {
        public bool IsValid { get; set; }
        public string? Error { get; set; }
        public GoogleUserInfo? UserInfo { get; set; }
    }
}