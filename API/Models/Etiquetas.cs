public class Etiqueta
{
    public int EtiquetaID { get; set; }
    public string Nombre { get; set; }

    public ICollection<RecetaEtiquetas> Recetas { get; set; }
}
