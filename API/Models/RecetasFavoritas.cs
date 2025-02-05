public class RecetasFavoritas
{
    public int UsuarioID { get; set; }
    public Usuario Usuario { get; set; }

    public int RecetaID { get; set; }
    public Receta Receta { get; set; }

    public DateTime FechaAgregado { get; set; } = DateTime.Now;
}
