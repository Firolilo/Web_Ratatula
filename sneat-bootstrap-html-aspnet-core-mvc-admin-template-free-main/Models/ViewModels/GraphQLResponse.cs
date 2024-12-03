namespace AspnetCoreMvcFull.Models.ViewModels
{
  public class GraphQLResponse
  {
    public Data data { get; set; }
  }

  public class Data
  {
    public List<ProductoViewModel> ObtenerProductos { get; set; }
  }
}
