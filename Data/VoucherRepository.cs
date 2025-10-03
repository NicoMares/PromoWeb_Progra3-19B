using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Data
{
    public class VoucherRepository
    {
        public Voucher ObtenerPorCodigo(string codigo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT CodigoVoucher, IdCliente, FechaCanje, IdArticulo FROM Vouchers WHERE CodigoVoucher = @c");
                datos.setearParametro("@c", codigo);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Voucher aux = new Voucher();
                    aux.CodigoVoucher = (string)datos.Lector["CodigoVoucher"];

                    if (!(datos.Lector["IdCliente"] is DBNull))
                    {
                        aux.IdCliente = (int)datos.Lector["IdCliente"];
                    }

                    if (!(datos.Lector["FechaCanje"] is DBNull))
                    {
                        aux.FechaCanje = (DateTime)datos.Lector["FechaCanje"];
                    }

                    if (!(datos.Lector["IdArticulo"] is DBNull))
                    {
                        aux.IdArticulo = (int)datos.Lector["IdArticulo"];
                    }

                    return aux;
                }
                return null;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        public void MarcarUsado(string codigo, int idCliente, int idArticulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Vouchers SET IdCliente=@cli, IdArticulo=@art, FechaCanje=GETDATE() WHERE CodigoVoucher=@c");
                datos.setearParametro("@cli", idCliente);
                datos.setearParametro("@art", idArticulo);
                datos.setearParametro("@c", codigo);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
