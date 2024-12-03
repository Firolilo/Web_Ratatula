using AspnetCoreMvcFull.Models;

public class UserSingleton
{
  private static UserSingleton _instance;

  public string Id { get; set; }
  public string Nombre { get; set; }
  public string Apellido { get; set; }
  public string Correo { get; set; }
  public string rol { get; set; }
  public string Estado { get; set; }
  public string Telefono { get; set; }
  public DateTime? FechaRegistro { get; set; }
  public string Descripcion { get; set; }
  public List<string> Categoria { get; set; }
  public Horarios Horarios { get; set; }

  private UserSingleton() { }

  public static UserSingleton Instance => _instance ??= new UserSingleton();
}
