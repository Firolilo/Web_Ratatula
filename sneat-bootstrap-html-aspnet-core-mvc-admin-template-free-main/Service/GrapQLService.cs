using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System.Threading.Tasks;

namespace AspnetCoreMvcFull.Service
{
    public class GraphQLService
    {
        private readonly GraphQLHttpClient _client;

        public GraphQLService(string endpoint)
        {
            _client = new GraphQLHttpClient(endpoint, new NewtonsoftJsonSerializer());
        }

        /// <summary>
        /// Ejecuta una consulta o mutación GraphQL con un token opcional de autorización.
        /// </summary>
        /// <typeparam name="T">Tipo de la respuesta esperada.</typeparam>
        /// <param name="query">La consulta o mutación GraphQL.</param>
        /// <param name="variables">Variables para la consulta o mutación.</param>
        /// <param name="token">Token de autorización opcional.</param>
        /// <returns>Datos de la respuesta GraphQL.</returns>
        public async Task<T> ExecuteQueryAsync<T>(string query, object variables = null, string token = null)
        {
            // Si se proporciona un token, agregarlo al encabezado Authorization
            if (!string.IsNullOrEmpty(token))
            {
                //Console.WriteLine($"Setting Authorization Header with token: {token}");

                // Remover cualquier encabezado Authorization existente para evitar duplicados
                if (_client.HttpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _client.HttpClient.DefaultRequestHeaders.Remove("Authorization");
                }

                // Agregar el encabezado Authorization manualmente
                _client.HttpClient.DefaultRequestHeaders.Add("Authorization", token);

                // Verificar que el encabezado se ha añadido
                if (_client.HttpClient.DefaultRequestHeaders.TryGetValues("Authorization", out var values))
                {
                    //Console.WriteLine($"Authorization Header set to: {string.Join(", ", values)}");
                }
                else
                {
                    Console.WriteLine("Failed to set Authorization Header.");
                }
            }

            var request = new GraphQLRequest
            {
                Query = query,
                Variables = variables
            };

            var response = await _client.SendQueryAsync<T>(request);

            // Si se estableció el token para esta solicitud, remover el encabezado para evitar afectar futuras solicitudes
            if (!string.IsNullOrEmpty(token))
            {
                _client.HttpClient.DefaultRequestHeaders.Remove("Authorization");
                //Console.WriteLine("Authorization Header removed after request.");
            }

            return response.Data;
        }
    }
}
