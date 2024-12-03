namespace AspnetCoreMvcFull.Models.ViewModels
{
  public class ProductoViewModel
  {
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string? IdLocal { get; set; }
  }
  public class ObtenerProductosResponse
  {
    public List<ProductoViewModel> ObtenerProductos { get; set; }
  }
}
