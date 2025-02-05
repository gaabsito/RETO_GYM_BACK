public class RecetaIngredientes
{
    public int RecetaID { get; set; }
    public Receta Receta { get; set; }

    public int IngredienteID { get; set; }
    public Ingrediente Ingrediente { get; set; }

    public decimal Cantidad { get; set; }
    
    public int UnidadID { get; set; }
    public UnidadMedida Unidad { get; set; }

    public string Notas { get; set; }
}
