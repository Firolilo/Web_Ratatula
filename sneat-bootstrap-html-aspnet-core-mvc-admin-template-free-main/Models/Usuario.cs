namespace AspnetCoreMvcFull.Models;
using System;
using System.Collections.Generic;

public class Usuario
{
  public Guid Id { get; set; }
  public string Nombre { get; set; }
  public string Apellido { get; set; }
  public string Correo { get; set; }
  public string Password { get; set; }
  public string Rol { get; set; } // "cliente" o "local"
  public DateTime FechaRegistro { get; set; }
  public string Estado { get; set; } // "activo" o "inactivo"
  public string Telefono { get; set; }
  public Guid? IdCarrito { get; set; } // Relación con CarritoCompras
  public string Descripcion { get; set; }
  public List<string> Categoria { get; set; } // Solo si el rol es "local"
  public Horarios Horarios { get; set; } // Solo si el rol es "local"
}

public class Horarios
{
  public string Apertura { get; set; }
  public string Cierre { get; set; }
}
