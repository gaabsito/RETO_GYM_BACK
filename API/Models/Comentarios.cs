public class Comentario
{
    public int ComentarioID { get; set; }

    public int RecetaID { get; set; }
    public Receta Receta { get; set; }

    public int UsuarioID { get; set; }
    public Usuario Usuario { get; set; }

    public string Contenido { get; set; }
    public int Calificacion { get; set; } // 1 a 5 estrellas

    public DateTime FechaComentario { get; set; } = DateTime.Now;
}
