using Dominio;
using System;
using System.Collections.Generic;

namespace Data
{
    public class ArticuloRepository
    {
        public List<Articulo> Listar()
        {
            var lista = new List<Articulo>();
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT Id, Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio " +
                    "FROM Articulos ORDER BY Nombre"
                );
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    var aux = new Articulo
                    {
                        Id = (int)datos.Lector["Id"],
                        Codigo = datos.Lector["Codigo"] is DBNull ? "" : (string)datos.Lector["Codigo"],
                        Nombre = datos.Lector["Nombre"] is DBNull ? "" : (string)datos.Lector["Nombre"],
                        Descripcion = datos.Lector["Descripcion"] is DBNull ? "" : (string)datos.Lector["Descripcion"],
                        IdMarca = datos.Lector["IdMarca"] is DBNull ? 0 : (int)datos.Lector["IdMarca"],
                        IdCategoria = datos.Lector["IdCategoria"] is DBNull ? 0 : (int)datos.Lector["IdCategoria"],
                        Precio = datos.Lector["Precio"] is DBNull ? 0m : (decimal)datos.Lector["Precio"],
                    };

                    // Carga TODAS las imágenes del artículo (y agrega fallback si no hay)
                    aux.Imagenes = ListarImagenes(aux.Id);
                    if (aux.Imagenes == null || aux.Imagenes.Count == 0)
                        aux.Imagenes = new List<Imagen> { new Imagen { ImagenUrl = "img/noimage.png" } };

                    lista.Add(aux);
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        private List<Imagen> ListarImagenes(int idArticulo)
        {
            var lista = new List<Imagen>();
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT Id, IdArticulo, ImagenUrl " +
                    "FROM Imagenes WHERE IdArticulo=@id ORDER BY Id"
                );
                datos.setearParametro("@id", idArticulo);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    // Evita nulos en URL
                    var url = datos.Lector["ImagenUrl"] is DBNull ? "" : (string)datos.Lector["ImagenUrl"];
                    if (string.IsNullOrWhiteSpace(url)) continue;

                    lista.Add(new Imagen
                    {
                        Id = (int)datos.Lector["Id"],
                        IdArticulo = (int)datos.Lector["IdArticulo"],
                        ImagenUrl = url
                    });
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Nota: el parámetro es realmente el ID del artículo, no "código".
        public string ObtenerNombrePorCodigo(int codigoArticulo)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT Nombre FROM Articulos WHERE Id = @codigo");
                datos.setearParametro("@codigo", codigoArticulo);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                    return datos.Lector["Nombre"] is DBNull ? "" : (string)datos.Lector["Nombre"];

                throw new Exception("Artículo no encontrado.");
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
