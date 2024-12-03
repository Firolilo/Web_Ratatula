using System.Collections.Generic;

namespace AspnetCoreMvcFull.Models.ViewModels
{
  // Respuesta para ObtenerPromocionesLocal
  public class ObtenerPromocionesLocalResponse
  {
    public List<Promocion> ObtenerPromocionesLocal { get; set; }
  }

  public class Promocion
  {
    public string Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public float PrecioReal { get; set; }
    public float PrecioPromo { get; set; }
    public List<ProductoEnPromocion> Productos { get; set; }
    public string IdLocal { get; set; }
  }

  public class ProductoEnPromocion
  {
    public string IdProducto { get; set; }
    public int Cantidad { get; set; }
  }

  // Respuesta para NuevaPromocion
  public class NuevaPromocionResponse
  {
    public Promocion NuevaPromocion { get; set; }
  }

  // Respuesta para EliminarPromocion
  public class EliminarPromocionResponse
  {
    public string EliminarPromocion { get; set; }
  }
}
