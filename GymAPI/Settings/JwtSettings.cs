namespace GymAPI.Settings;

public class JwtSettings
{
    public required string SecretKey { get; set; }
    public int ExpirationMinutes { get; set; } = 1440; // 24 horas por defecto
}