namespace AspnetCoreMvcFull.Models;

public class ProductoResponse
{
  public Producto NuevoProducto { get; set; }
}

public class Producto
{
  public string Descripcion { get; set; }
  public string Id { get; set; }
  public string IdLocal { get; set; }
  public string Nombre { get; set; }
  public decimal Precio { get; set; }
  public int Stock { get; set; }
}
