using AspnetCoreMvcFull.Models.ViewModels;
using AspnetCoreMvcFull.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using AspnetCoreMvcFull.Models;

namespace AspnetCoreMvcFull.Controllers
{
    public class ProductoController : Controller
    {
        private readonly GraphQLService _graphQLService;

        public ProductoController(GraphQLService graphQLService)
        {
            _graphQLService = graphQLService;
        }

        // Acción para mostrar la vista de productos
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Definimos la consulta GraphQL para obtener productos
            const string query = @"
                query ObtenerProductos {
                    obtenerProductos {
                        id
                        nombre
                        descripcion
                        precio
                        stock
                        idLocal
                    }
                }";

            // Llamamos al servicio GraphQL para obtener los productos
            var response = await _graphQLService.ExecuteQueryAsync<ObtenerProductosResponse>(query);

            // Imprimir la respuesta de GraphQL en la consola
            Console.WriteLine("Respuesta completa de la consulta GraphQL:");
            var jsonResponse = JsonConvert.SerializeObject(response, Formatting.Indented);
            Console.WriteLine(jsonResponse);

            return View(response.ObtenerProductos);
            // Pasamos los productos a la vista
        }

        // Acción para mostrar la vista de crear producto
        public IActionResult Create()
        {
            return View(); // Asegúrate de tener la vista Create.cshtml en Views/Producto/
        }

        // Acción para crear un nuevo producto usando GraphQL
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductoViewModel productoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            const string query = @"
                mutation NuevoProducto($input: ProductoInput) {
                    nuevoProducto(input: $input) {
                        descripcion
                        id
                        idLocal
                        nombre
                        precio
                        stock
                    }
                }";

            var variables = new
            {
                input = new
                {
                    nombre = productoViewModel.Nombre,
                    descripcion = productoViewModel.Descripcion,
                    precio = productoViewModel.Precio,
                    stock = productoViewModel.Stock,
                    idLocal = UserSingleton.Instance.Id
                }
            };

            try
            {
                // Obtener el token de la sesión
                var token = HttpContext.Session.GetString("AuthToken");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("No se ha encontrado un token de autorización.");
                }

                // Llamar a ExecuteQueryAsync pasando el token
                var response = await _graphQLService.ExecuteQueryAsync<ProductoResponse>(query, variables, token);

                // Verificar la respuesta y mostrarla en la consola
                Console.WriteLine("Respuesta completa de la mutación GraphQL para crear producto:");
                var jsonResponse = JsonConvert.SerializeObject(response, Formatting.Indented);
                Console.WriteLine(jsonResponse);

                // Verificar si se recibió el producto correctamente
                if (response?.NuevoProducto != null)
                {
                    Console.WriteLine("Producto creado con éxito:");
                    Console.WriteLine($"ID: {response.NuevoProducto.Id}");
                    Console.WriteLine($"Nombre: {response.NuevoProducto.Nombre}");
                    Console.WriteLine($"Descripción: {response.NuevoProducto.Descripcion}");
                    Console.WriteLine($"Precio: {response.NuevoProducto.Precio}");
                    Console.WriteLine($"Stock: {response.NuevoProducto.Stock}");
                    Console.WriteLine($"ID Local: {response.NuevoProducto.IdLocal}");

                    return RedirectToAction("Index"); // Redirigir a la vista Index con los productos actualizados
                }
                else
                {
                    Console.WriteLine("No se recibió el producto en la respuesta.");
                    ModelState.AddModelError("", "Hubo un problema al crear el producto.");
                    return View(productoViewModel); // Vuelve a mostrar la vista para corregir el error
                }
            }
            catch (Exception ex)
            {
                // Manejar errores
                Console.WriteLine($"Error al crear el producto: {ex.Message}");
                ModelState.AddModelError("", $"Error al crear el producto: {ex.Message}");
                return View(productoViewModel); // Vuelve a mostrar la vista con el error
            }
        }
    }
}
