using Newtonsoft.Json;

namespace AspnetCoreMvcFull.Models.ViewModels
{

  public class UsuarioResponse
  {
    [JsonProperty("data")]
    public UsuarioData Data { get; set; }
  }

  public class UsuarioData
  {
    [JsonProperty("obtenerUsuarioPorID")]
    public UsuarioDetails Usuario { get; set; }
  }

  public class UsuarioDetails
  {
    [JsonProperty("id")]
    public string UsuarioId { get; set; }

    [JsonProperty("nombre")]
    public string Nombre { get; set; }

    [JsonProperty("apellido")]
    public string Apellido { get; set; }

    [JsonProperty("correo")]
    public string Correo { get; set; }

    [JsonProperty("rol")]
    public string Rol { get; set; }

    [JsonProperty("estado")]
    public string Estado { get; set; }

    [JsonProperty("telefono")]
    public string Telefono { get; set; }

    [JsonProperty("fechaRegistro")]
    public string FechaRegistro { get; set; }

    [JsonProperty("descripcion")]
    public string Descripcion { get; set; }

    [JsonProperty("categoria")]
    public List<string> Categorias { get; set; }

    [JsonProperty("horarios")]
    public HorarioDetails Horarios { get; set; }
  }

  public class HorarioDetails
  {
    [JsonProperty("apertura")]
    public string HoraApertura { get; set; }

    [JsonProperty("cierre")]
    public string HoraCierre { get; set; }
  }


}
