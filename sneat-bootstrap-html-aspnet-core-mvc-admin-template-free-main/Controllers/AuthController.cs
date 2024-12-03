namespace AspnetCoreMvcFull.Controllers
{
  using AspnetCoreMvcFull.Models.ViewModels;
  using AspnetCoreMvcFull.Service;
  using Microsoft.AspNetCore.Mvc;
  using Newtonsoft.Json;
  using System;
  using System.Threading.Tasks;

  [Route("auth")]
    public class AuthController : Controller
    {
        private readonly GraphQLService _graphQLService;

        public AuthController(GraphQLService graphQLService)
        {
            _graphQLService = graphQLService;
        }

        // Acción que maneja la vista del login
        [HttpGet]
        public IActionResult LoginBasic()
        {
            return View(); // Asegúrate de tener la vista LoginBasic.cshtml en Views/Auth/
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            const string query = @"
                mutation AutenticarUsuario($input: inputAutenticar) {
                    autenticarUsuario(input: $input) {
                        token
                    }
                }";

            var variables = new
            {
                input = new
                {
                    correo = loginViewModel.Correo,
                    password = loginViewModel.Password
                }
            };

            try
            {
                // Ejecutar la mutación para autenticar al usuario
                var response = await _graphQLService.ExecuteQueryAsync<LoginResponse>(query, variables);

                // Extraer el token desde la respuesta
                var token = response?.AutenticarUsuario?.Token;

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Correo o contraseña incorrectos.");
                }

                await GetUserIdFromToken(token);
                // Almacenar el token en la sesión
                HttpContext.Session.SetString("AuthToken", token);
                Console.WriteLine($"Token almacenado en sesión: {token}");

                // Redirigir a la página deseada después del login exitoso
                return RedirectToAction("Index", "Dashboards");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al autenticar el usuario: {ex.Message}");
            }
        }

    private async Task<string> GetUserIdFromToken(string token)
    {
      // Consulta GraphQL para obtener el usuario con el token
      const string query = @"
                query ObtenerUsuario($token: String) {
                    obtenerUsuario(token: $token) {
                        id
                    }
                }";

      var variables = new
      {
        token = token
      };

      try
      {
        // Realizar la consulta GraphQL
        var response = await _graphQLService.ExecuteQueryAsync<ObtenerUsuarioIdResponseViewModel>(query, variables);

        // Verificar si la respuesta contiene el id
        var id = response?.obtenerUsuario?.id;

        if (string.IsNullOrEmpty(id))
        {
          throw new Exception("No se pudo obtener el id del usuario.");
        }

        await PostLoginActions(id);
        return id;
      }
      catch (Exception ex)
      {
        // Manejo de errores
        Console.WriteLine($"Error al obtener el id del usuario: {ex.Message}");
        throw;
      }
    }

    private async Task PostLoginActions(string id)
    {
      // Crear una instancia de TokenService
      var tokenService = new TokenService(new HttpClient());

      // Definir la consulta GraphQL
      string query = @"
    query ObtenerUsuarioPorID($obtenerUsuarioPorIdId: ID!) {
        obtenerUsuarioPorID(id: $obtenerUsuarioPorIdId) {
            id
            nombre
            apellido
            correo
            rol
            estado
            telefono
            fechaRegistro
            descripcion
            categoria
            horarios {
                apertura
                cierre
            }
        }
    }";

      // Variables para la consulta GraphQL
      var variables = new { obtenerUsuarioPorIdId = id };

      // Obtener y mapear los datos del usuario
      await tokenService.GetUserDetailsFromResponse(query, variables);

      // Aquí ya puedes acceder a los datos del usuario a través del UserSingleton
      var usuario = UserSingleton.Instance;
      Console.WriteLine($"ID: {usuario.Id}, Nombre: {usuario.Nombre}, Correo: {usuario.Correo}, Rol: {usuario.rol}");
    }
  }
}
