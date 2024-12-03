using AspnetCoreMvcFull.Models.ViewModels;
using AspnetCoreMvcFull.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AspnetCoreMvcFull.Controllers
{
    public class PromocionesController : Controller
    {
        private readonly GraphQLService _graphQLService;

        public PromocionesController(GraphQLService graphQLService)
        {
            _graphQLService = graphQLService;
        }

        // Acción para ver promociones
        [HttpGet]
        public async Task<IActionResult> Ver()
        {
            // Define la consulta GraphQL para obtener promociones de un local específico
            const string query = @"
                query ObtenerPromocionesLocal($idLocal: ID!) {
                    obtenerPromocionesLocal(idLocal: $idLocal) {
                        id
                        nombre
                        descripcion
                        precioReal
                        precioPromo
                        productos {
                            idProducto
                            cantidad
                        }
                        imagen
                        idLocal
                    }
                }";

            // Supongamos que el idLocal se obtiene del usuario autenticado o de otra fuente
            // Aquí lo obtenemos de la sesión, pero ajusta esto según tu lógica
            var idLocal = UserSingleton.Instance.Id;
            if (string.IsNullOrEmpty(idLocal))
            {
                return Unauthorized("No se encontró el ID del local en la sesión.");
            }

            var variables = new
            {
                idLocal = idLocal
            };

            try
            {
                // Ejecutar la consulta GraphQL
                var response = await _graphQLService.ExecuteQueryAsync<ObtenerPromocionesLocalResponse>(query, variables, HttpContext.Session.GetString("AuthToken"));

                // Verificar la respuesta y pasar los datos a la vista
                if (response?.ObtenerPromocionesLocal != null)
                {
                    return View(response.ObtenerPromocionesLocal);
                }
                else
                {
                    ViewBag.Mensaje = "No se encontraron promociones para este local.";
                    return View(new List<Promocion>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener promociones: {ex.Message}");
                return StatusCode(500, $"Error al obtener promociones: {ex.Message}");
            }
        }

        // Acción para mostrar la vista de crear promoción (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Asegúrate de tener la vista Create.cshtml en Views/Promociones/
        }

        // Acción para crear una nueva promoción (POST)
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PromocionViewModel promocionViewModel)
        {
          if (!ModelState.IsValid)
          {
            foreach (var key in ModelState.Keys)
            {
              var value = ModelState[key].Errors;
              Console.WriteLine($"{key}: {string.Join(", ", value.Select(e => e.ErrorMessage))}");
            }

            return View(promocionViewModel);
          }

            // Define la mutación GraphQL para crear una nueva promoción
            const string mutation = @"
                mutation NuevaPromocion($input: PromocionInput!) {
                    nuevaPromocion(input: $input) {
                        id
                        nombre
                        descripcion
                        precioReal
                        precioPromo
                        productos {
                            idProducto
                            cantidad
                        }
                        idLocal
                    }
                }";

            var variables = new
            {
                input = new
                {
                    nombre = promocionViewModel.Nombre,
                    descripcion = promocionViewModel.Descripcion,
                    precioPromo = promocionViewModel.PrecioPromo,
                    productos = promocionViewModel.Productos.Select(p => new
                    {
                        idProducto = p.IdProducto,
                        cantidad = p.Cantidad
                    }).ToList(),
                    idLocal = UserSingleton.Instance.Id
                }
            };

            try
            {
                var token = HttpContext.Session.GetString("AuthToken");

                var response = await _graphQLService.ExecuteQueryAsync<NuevaPromocionResponse>(mutation, variables, token);
                Console.WriteLine("Respuesta de GraphQL: " + JsonConvert.SerializeObject(response));


                // Verificar la respuesta y redirigir
                if (response?.NuevaPromocion != null)
                {
                  Console.WriteLine("Se creo bien");
                  Console.WriteLine(response.NuevaPromocion);
                    return RedirectToAction("Ver");
                }
                else
                {
                    Console.WriteLine("Error al crear la promocion");
                    return View(promocionViewModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la promoción: {ex.Message}");
                ModelState.AddModelError(string.Empty, $"Error al crear la promoción: {ex.Message}");
                return View(promocionViewModel);
            }
        }

        // Acción para eliminar una promoción (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID de promoción no proporcionado.");
            }

            // Define la mutación GraphQL para eliminar una promoción
            const string mutation = @"
                mutation EliminarPromocion($id: ID!) {
                    eliminarPromocion(id: $id)
                }";

            var variables = new
            {
                id = id
            };

            try
            {
                // Ejecutar la mutación GraphQL
                var response = await _graphQLService.ExecuteQueryAsync<EliminarPromocionResponse>(mutation, variables, HttpContext.Session.GetString("AuthToken"));

                // Verificar la respuesta y redirigir
                if (response?.EliminarPromocion == "Promoción eliminada!")
                {
                    return RedirectToAction("Ver");
                }
                else
                {
                    return StatusCode(500, "No se pudo eliminar la promoción.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la promoción: {ex.Message}");
                return StatusCode(500, $"Error al eliminar la promoción: {ex.Message}");
            }
        }
    }
}
