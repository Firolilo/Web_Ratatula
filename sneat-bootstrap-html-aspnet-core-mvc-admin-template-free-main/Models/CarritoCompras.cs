
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class CarritoCompras
{
  public Guid Id { get; set; }
  public Guid IdUsuario { get; set; } // Relación con Usuario
  public List<CarritoProducto> Productos { get; set; } = new List<CarritoProducto>();
  public List<CarritoPromocion> Promociones { get; set; } = new List<CarritoPromocion>();
  public decimal Total { get; set; }
  public DateTime FechaActualizacion { get; set; }
  public Guid? IdLocal { get; set; } // Relación con Usuario (Local)
}

public class CarritoProducto
{
  public Guid IdProducto { get; set; } // Relación con Producto
  public int Cantidad { get; set; }
}

public class CarritoPromocion
{
  public Guid IdPromo { get; set; } // Relación con Promoción
  public int Cantidad { get; set; }
}
