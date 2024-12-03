using AspnetCoreMvcFull.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class TokenService
{
  private readonly HttpClient _httpClient;

  public TokenService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task GetUserDetailsFromResponse(string query, object variables)
  {
    try
    {
      // Realiza la solicitud POST a la API GraphQL
      var content = new StringContent(
          JsonConvert.SerializeObject(new { query, variables }),
          System.Text.Encoding.UTF8,
          "application/json"
      );

      // Hacer la solicitud
      var response = await _httpClient.PostAsync("http://localhost:4000/graphql", content);

      // Asegurarse de que la respuesta sea exitosa
      response.EnsureSuccessStatusCode();

      // Leer el contenido de la respuesta
      var responseBody = await response.Content.ReadAsStringAsync();

      // Parsear el JSON de la respuesta
      var jsonResponse = JObject.Parse(responseBody);

      // Extraer la respuesta completa de los datos
      var userDetails = jsonResponse["data"]?["obtenerUsuarioPorID"];

      if (userDetails != null)
      {
        // Mapear los datos a la instancia de UserSingleton
        var userSingleton = UserSingleton.Instance;

        userSingleton.Id = userDetails["id"]?.ToString();
        userSingleton.Nombre = userDetails["nombre"]?.ToString();
        userSingleton.Apellido = userDetails["apellido"]?.ToString();
        userSingleton.Correo = userDetails["correo"]?.ToString();
        userSingleton.rol = userDetails["rol"]?.ToString();
        userSingleton.Estado = userDetails["estado"]?.ToString();
        userSingleton.Telefono = userDetails["telefono"]?.ToString();
        userSingleton.FechaRegistro = DateTime.TryParse(userDetails["fechaRegistro"]?.ToString(), out var fechaRegistro) ? fechaRegistro : (DateTime?)null;
        userSingleton.Descripcion = userDetails["descripcion"]?.ToString();
        userSingleton.Categoria = userDetails["categoria"]?.ToObject<List<string>>();

        // Mapear horarios
        var horarios = userDetails["horarios"];
        if (horarios != null)
        {
          userSingleton.Horarios = new Horarios
          {
            Apertura = horarios["apertura"]?.ToString(),
            Cierre = horarios["cierre"]?.ToString()
          };
        }
      }
    }
    catch (Exception ex)
    {
      // Manejo de excepciones
      Console.WriteLine($"Error al obtener los detalles del usuario: {ex.Message}");
    }
  }
}
