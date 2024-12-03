namespace AspnetCoreMvcFull.Models.ViewModels;

public class ObtenerPedidosCompletadosResponse
{
  public List<Pedido> ObtenerPedidosCompletados { get; set; }
}

public class Pedido
{
  public string Id { get; set; }
  public Cliente Cliente { get; set; }
  public List<ProductoPedido> Productos { get; set; }
  public float Total { get; set; }
  public string Fecha { get; set; }
}

public class Cliente
{
  public string Id { get; set; }
  public string Nombre { get; set; }
  public string Correo { get; set; }
}

public class ProductoPedido
{
  public string Id { get; set; }
  public string Nombre { get; set; }
  public int Cantidad { get; set; }
  public float Precio { get; set; }
}
