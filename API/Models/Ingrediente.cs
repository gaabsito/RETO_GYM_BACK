public class Ingrediente
{
    public int IngredienteID { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public int UnidadPredeterminada { get; set; }

    public UnidadMedida Unidad { get; set; }
    public ICollection<RecetaIngredientes> RecetaIngredientes { get; set; }
}
