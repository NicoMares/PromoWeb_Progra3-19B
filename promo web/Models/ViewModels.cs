using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Promos.Web.Models
{
    // Paso 1
    public class Step1Vm
    {
        [Required, MaxLength(50)]
        public string Codigo { get; set; } = "";
    }

    // Paso 2
    public class Step2Vm
    {
        public string Codigo { get; set; } = "";
        public int? SelectedArticuloId { get; set; }
        public List<ArticuloCard> Articulos { get; set; } = new();

        public class ArticuloCard
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = "";
            public string? Descripcion { get; set; }
            public string? PrimeraImagenUrl { get; set; }
        }
    }

    // Paso 3
    public class Step3Vm
    {
        [Required] public string Codigo { get; set; } = null!;
        [Required] public int SelectedArticuloId { get; set; }

        [Required] public string Documento { get; set; } = null!;
        [Required] public string Nombre { get; set; } = null!;
        [Required] public string Apellido { get; set; } = null!;
        [Required, EmailAddress] public string Email { get; set; } = null!;
        [Required] public string Direccion { get; set; } = null!;
        [Required] public string Ciudad { get; set; } = null!;
        [Required] public int CP { get; set; }
    }

    // Éxito
    public class SuccessVm
    {
        public string Codigo { get; set; } = "";
        public string Premio { get; set; } = "";
        public string NombreCompleto { get; set; } = "";
    }
}
