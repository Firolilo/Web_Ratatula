using System;
using System.Text;
using Newtonsoft.Json;

public class JwtDecoder
{
  public static string GetIdFromToken(string token)
  {
    try
    {
      // El JWT está compuesto por tres partes separadas por puntos
      var parts = token.Split('.');
      if (parts.Length != 3)
      {
        Console.WriteLine("El token JWT es inválido.");
        return null;
      }

      // Decodificar la parte del 'payload' (Base64Url -> Base64 -> string)
      var payload = parts[1];
      var decodedPayload = Base64UrlDecode(payload);

      // Deserializar el payload en un diccionario
      var payloadObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(decodedPayload);

      // Verificar si el 'id' está presente en el payload y devolverlo
      if (payloadObj.ContainsKey("id"))
      {
        return payloadObj["id"].ToString();
      }

      return null; // Si no se encuentra el 'id', retornamos null
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error al decodificar el token: {ex.Message}");
      return null;
    }
  }

  // Método para decodificar Base64Url (utilizado por JWT)
  private static string Base64UrlDecode(string base64Url)
  {
    // Reemplazar los caracteres base64url para convertirlos a base64 estándar
    base64Url = base64Url.Replace('-', '+').Replace('_', '/');

    // Asegurarse de que la longitud del string sea múltiplo de 4
    int padding = 4 - base64Url.Length % 4;
    if (padding != 4)
    {
      base64Url = base64Url.PadRight(base64Url.Length + padding, '=');
    }

    byte[] decodedBytes = Convert.FromBase64String(base64Url);
    return Encoding.UTF8.GetString(decodedBytes);
  }
}
