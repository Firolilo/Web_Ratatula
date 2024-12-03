using AspnetCoreMvcFull.Service;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Habilitar los logs de PII para depuración
IdentityModelEventSource.ShowPII = true;

// Configura el servicio de sesión
builder.Services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromMinutes(30); // Configura el tiempo de expiración de la sesión
  options.Cookie.HttpOnly = true; // Hace que la cookie sea accesible solo por el servidor
  options.Cookie.IsEssential = true; // Necesario para cumplir con GDPR
});

// Agregar IHttpContextAccessor como un servicio
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Agregar servicios para MVC
builder.Services.AddControllersWithViews();

// Configurar GraphQLService como Scoped (por cada solicitud HTTP)
builder.Services.AddScoped<GraphQLService>(provider =>
{
  // Obtener IHttpContextAccessor desde el contenedor de servicios
  var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();

  // Crear y devolver una nueva instancia de GraphQLService con ambos parámetros
  return new GraphQLService("http://34.16.100.239:4000/graphql");
});

var app = builder.Build();

// Redirigir automáticamente a /auth cuando la aplicación se inicie
app.MapGet("/", () => Results.Redirect("/auth"));

// Configuración de la tubería de solicitud HTTP
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Habilitar sesiones
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Acces}/{action=Index}/{id?}");

app.Run();

