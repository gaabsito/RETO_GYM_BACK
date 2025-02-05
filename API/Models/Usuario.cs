public class Usuario
{
    public int UsuarioID { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    public bool EstaActivo { get; set; } = true;

    public ICollection<Receta> Recetas { get; set; }
    public ICollection<RecetasFavoritas> Favoritos { get; set; }
    public ICollection<Comentario> Comentarios { get; set; }
}
