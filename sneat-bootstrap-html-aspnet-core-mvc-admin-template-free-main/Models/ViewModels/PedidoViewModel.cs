using System;
using System.Collections.Generic;

namespace AspnetCoreMvcFull.Models.ViewModels
{
  // ViewModel para ProductoEnPedido
  public class ProductoEnPedidoViewModel
  {
    public string IdProducto { get; set; }
    public int Cantidad { get; set; }
  }

  // ViewModel para PromocionEnPedido
  public class PromocionEnPedidoViewModel
  {
    public string IdPromo { get; set; }
    public int Cantidad { get; set; }
  }

  // ViewModel para Pedido
  public class PedidoViewModel
  {
    public string Id { get; set; }
    public string IdCliente { get; set; }
    public string IdLocal { get; set; }
    public List<ProductoEnPedidoViewModel> Productos { get; set; }
    public List<PromocionEnPedidoViewModel> Promociones { get; set; }
    public int TiempoPreparacion { get; set; }
    public string Estado { get; set; }
    public string EstadoVenta { get; set; }
    public float Total { get; set; }
    public string FechaCreacion { get; set; }
    public string Urlqr { get; set; }
    public bool Qr { get; set; }
    public string TransaccionID { get; set; }
  }

  // Respuesta para obtenerPedidosLocalPendiente
  public class ObtenerPedidosLocalPendienteResponse
  {
    public List<PedidoViewModel> ObtenerPedidosLocalPendiente { get; set; }
  }

  // Respuesta para obtenerPedidosLocalCompletado
  public class ObtenerPedidosLocalCompletadoResponse
  {
    public List<PedidoViewModel> ObtenerPedidosLocalCompletado { get; set; }
  }
}
