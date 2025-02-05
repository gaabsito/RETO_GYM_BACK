public class PasoPreparacion
{
    public int PasoID { get; set; }
    
    public int RecetaID { get; set; }
    public Receta Receta { get; set; }

    public int NumeroPaso { get; set; }
    public string Descripcion { get; set; }
    public string ImagenURL { get; set; }
}
