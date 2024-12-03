using AspnetCoreMvcFull.Models.ViewModels;
using AspnetCoreMvcFull.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AspnetCoreMvcFull.Controllers
{
    public class PedidosController : Controller
    {
        private readonly GraphQLService _graphQLService;

        public PedidosController(GraphQLService graphQLService)
        {
            _graphQLService = graphQLService;
        }

        // Acción para ver pedidos pendientes
        [HttpGet]
        public async Task<IActionResult> Pendientes()
        {
            // Define la consulta GraphQL para obtener pedidos pendientes de un local específico
            const string query = @"
                query ObtenerPedidosLocalPendiente($id: ID!) {
                    obtenerPedidosLocalPendiente(id: $id) {
                        id
                        idCliente
                        idLocal
                        productos {
                            idProducto
                            cantidad
                        }
                        promociones {
                            idPromo
                            cantidad
                        }
                        tiempoPreparacion
                        estado
                        estadoVenta
                        total
                        fechaCreacion
                        urlqr
                        qr
                        transaccionID
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
                id = idLocal
            };

            try
            {
                // Ejecutar la consulta GraphQL
                var response = await _graphQLService.ExecuteQueryAsync<ObtenerPedidosLocalPendienteResponse>(query, variables, HttpContext.Session.GetString("AuthToken"));

                // Verificar la respuesta y pasar los datos a la vista
                if (response?.ObtenerPedidosLocalPendiente != null)
                {
                    return View(response.ObtenerPedidosLocalPendiente);
                }
                else
                {
                    ViewBag.Mensaje = "No se encontraron pedidos pendientes para este local.";
                    return View(new List<PedidoViewModel>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener pedidos pendientes: {ex.Message}");
                return StatusCode(500, $"Error al obtener pedidos pendientes: {ex.Message}");
            }
        }

        // Acción para ver pedidos completados
        [HttpGet]
        public async Task<IActionResult> Completados()
        {
            // Define la consulta GraphQL para obtener pedidos completados de un local específico
            const string query = @"
                query ObtenerPedidosLocalCompletado($id: ID!) {
                    obtenerPedidosLocalCompletado(id: $id) {
                        id
                        idCliente
                        idLocal
                        productos {
                            idProducto
                            cantidad
                        }
                        promociones {
                            idPromo
                            cantidad
                        }
                        tiempoPreparacion
                        estado
                        estadoVenta
                        total
                        fechaCreacion
                        urlqr
                        qr
                        transaccionID
                    }
                }";

            // Obtiene el idLocal de la sesión
            var idLocal = UserSingleton.Instance.Id;

            if (string.IsNullOrEmpty(idLocal))
            {
                return Unauthorized("No se encontró el ID del local en la sesión.");
            }

            var variables = new
            {
                id = idLocal
            };

            try
            {
                // Ejecutar la consulta GraphQL
                var response = await _graphQLService.ExecuteQueryAsync<ObtenerPedidosLocalCompletadoResponse>(query, variables, HttpContext.Session.GetString("AuthToken"));

                // Verificar la respuesta y pasar los datos a la vista
                if (response?.ObtenerPedidosLocalCompletado != null)
                {
                    return View(response.ObtenerPedidosLocalCompletado);
                }
                else
                {
                    ViewBag.Mensaje = "No se encontraron pedidos completados para este local.";
                    return View(new List<PedidoViewModel>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener pedidos completados: {ex.Message}");
                return StatusCode(500, $"Error al obtener pedidos completados: {ex.Message}");
            }
        }
    }
}
