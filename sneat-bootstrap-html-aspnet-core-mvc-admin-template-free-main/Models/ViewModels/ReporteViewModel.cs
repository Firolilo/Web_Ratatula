using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspnetCoreMvcFull.Models.ViewModels
{
  public class ReporteViewModel
  {
    [Required]
    public string Titulo { get; set; }

    public string Descripcion { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; }

    [Required]
    public string Tipo { get; set; }

    // Suponiendo que los datos son un diccionario de clave-valor
    public List<ReporteDatoViewModel> Datos { get; set; } = new List<ReporteDatoViewModel>();
  }

  public class ReporteDatoViewModel
  {
    [Required]
    public string Clave { get; set; }

    [Required]
    public string Valor { get; set; }
  }
}
