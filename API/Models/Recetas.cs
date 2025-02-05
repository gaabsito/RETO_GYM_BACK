public class Receta
{
    public int RecetaID { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public int TiempoPreparacion { get; set; } // En minutos
    public int Porciones { get; set; }
    public string Dificultad { get; set; } // Fácil, Media, Difícil
    public string ImagenURL { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    
    public int AutorID { get; set; }
    public Usuario Autor { get; set; }
    
    public int CategoriaID { get; set; }
    public Categoria Categoria { get; set; }

    public ICollection<RecetaIngredientes> Ingredientes { get; set; }
    public ICollection<PasoPreparacion> Pasos { get; set; }
    public ICollection<RecetaEtiquetas> Etiquetas { get; set; }
    public ICollection<RecetasFavoritas> Favoritos { get; set; }
    public ICollection<Comentario> Comentarios { get; set; }
}
