using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Data
{
    public class ClienteRepository
    {
        public Cliente ObtenerPorDocumento(string documento)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT Id, Documento, Nombre, Apellido, Email, Direccion, Ciudad, CP FROM Clientes WHERE Documento = @doc");
                datos.setearParametro("@doc", documento);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Cliente aux = new Cliente();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Documento = (string)datos.Lector["Documento"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Email = (string)datos.Lector["Email"];
                    aux.Direccion = (string)datos.Lector["Direccion"];
                    aux.Ciudad = (string)datos.Lector["Ciudad"];
                    aux.CodigoPostal = (int)datos.Lector["CP"];
                    return aux;
                }
                return null;
            }
            finally
            {

                datos.cerrarConexion();
            }
        }

        public int Insertar(Cliente cliente)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO Clientes (Documento, Nombre, Apellido, Email, Direccion, Ciudad, CP) " +
                                     "OUTPUT INSERTED.Id " +
                                     "VALUES (@doc, @nom, @ape, @mail, @dir, @ciu, @cp)");
                datos.setearParametro("@doc", cliente.Documento);
                datos.setearParametro("@nom", cliente.Nombre);
                datos.setearParametro("@ape", cliente.Apellido);
                datos.setearParametro("@mail", cliente.Email);
                datos.setearParametro("@dir", cliente.Direccion);
                datos.setearParametro("@ciu", cliente.Ciudad);
                datos.setearParametro("@cp", cliente.CodigoPostal);

                datos.ejecutarLectura();
                if (datos.Lector.Read())
                {
                    return (int)datos.Lector[0];
                }

                return 0;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Actualizar(Cliente cliente)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Clientes SET Nombre=@nom, Apellido=@ape, Email=@mail, Direccion=@dir, Ciudad=@ciu, CP=@cp WHERE Id=@id");
                datos.setearParametro("@nom", cliente.Nombre);
                datos.setearParametro("@ape", cliente.Apellido);
                datos.setearParametro("@mail", cliente.Email);
                datos.setearParametro("@dir", cliente.Direccion);
                datos.setearParametro("@ciu", cliente.Ciudad);
                datos.setearParametro("@cp", cliente.CodigoPostal);
                datos.setearParametro("@id", cliente.Id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
