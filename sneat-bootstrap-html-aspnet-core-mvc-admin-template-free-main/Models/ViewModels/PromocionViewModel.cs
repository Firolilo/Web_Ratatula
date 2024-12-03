using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspnetCoreMvcFull.Models.ViewModels
{
  public class PromocionViewModel
  {
    [Required]
    public string Nombre { get; set; }

    public string Descripcion { get; set; }

    [Required]
    public float PrecioPromo { get; set; }

    [Required]
    public List<ProductoEnPromocionViewModel> Productos { get; set; }
  }

  public class ProductoEnPromocionViewModel
  {
    [Required]
    public string IdProducto { get; set; }

    [Required]
    public int Cantidad { get; set; }
  }
}
