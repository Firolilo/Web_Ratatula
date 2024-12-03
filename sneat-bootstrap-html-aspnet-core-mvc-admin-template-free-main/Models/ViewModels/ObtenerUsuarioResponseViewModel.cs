using AspnetCoreMvcFull.Models;

public class ObtenerUsuarioResponseViewModel
{
  public UserViewModel ObtenerUsuario { get; set; }
}

  public class UserViewModel
  {
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Correo { get; set; }
    public string Rol { get; set; } // "cliente" o "local"
    public string Estado { get; set; } // "activo" o "inactivo"
    public string Telefono { get; set; }
    public DateTime FechaRegistro { get; set; }
    public string Descripcion { get; set; }
    public List<string> Categoria { get; set; } // Solo si el rol es "local"
    public Horarios Horarios { get; set; } // Solo si el rol es "local"
  }

