public class UnidadMedida
{
    public int UnidadID { get; set; }
    public string Nombre { get; set; }
    public string Abreviatura { get; set; }

    public ICollection<Ingrediente> Ingredientes { get; set; }
    public ICollection<RecetaIngredientes> RecetaIngredientes { get; set; }
}
