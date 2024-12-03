namespace AspnetCoreMvcFull.Models.ViewModels
{
  public class AutenticarUsuarioResponse
  {
    public string Token { get; set; }
  }

  public class LoginResponse
  {
    public AutenticarUsuarioResponse AutenticarUsuario { get; set; }
  }
}
