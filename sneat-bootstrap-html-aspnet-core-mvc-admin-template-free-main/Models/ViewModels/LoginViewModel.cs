namespace AspnetCoreMvcFull.Models.ViewModels
{
  using System.ComponentModel.DataAnnotations;

  // Modelo de datos para la vista de login
  public class LoginViewModel
  {
    [Required(ErrorMessage = "El correo es obligatorio.")]
    public string Correo { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; }
  }
}
