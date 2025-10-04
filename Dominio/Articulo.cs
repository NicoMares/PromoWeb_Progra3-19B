using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Dominio
{
    public class Articulo
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdMarca { get; set; }
        public int IdCategoria { get; set; }
        public decimal Precio { get; set; }
        public List<Imagen> Imagenes { get; set; } = new List<Imagen>();

        public string PrimeraImagenUrl =>
        Imagenes != null && Imagenes.Count > 0 ? Imagenes[0].ImagenUrl : "img/noimage.png";
    }
}

