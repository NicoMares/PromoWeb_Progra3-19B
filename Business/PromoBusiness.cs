using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Data;

namespace Business
{
    public class PromoBusiness
    {

        private readonly VoucherRepository _voucherRepo = new VoucherRepository();
        private readonly ClienteRepository _clienteRepo = new ClienteRepository();
        private readonly ArticuloRepository _articuloRepo = new ArticuloRepository();

      
        public Voucher ValidarVoucher(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new Exception("El código no puede estar vacío.");
            }

            var voucher = _voucherRepo.ObtenerPorCodigo(codigo);

            if (voucher == null)
            {
                throw new Exception("El voucher no existe.");
            }

            if (voucher.Usado)
            {
                throw new Exception("El voucher ya fue utilizado.");
            }

            return voucher;
        }

        // Listar todos los premios disponibles
        public List<Articulo> ListarPremios()
        {
            var lista = _articuloRepo.Listar();

            if (lista == null || lista.Count == 0)
            {
                throw new Exception("No hay premios disponibles por el momento.");
            }

            return lista;
        }

        
        public Cliente BuscarCliente(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
            {
                throw new Exception("El DNI no puede estar vacío.");
            }

            documento = documento.Trim();

            foreach (char c in documento)
            {
                if (!char.IsDigit(c))
                {
                    throw new Exception("El DNI debe contener solo números.");
                }
            }

            return _clienteRepo.ObtenerPorDocumento(documento);
        }

        // Validar datos cliente
        private void ValidarClienteObligatorio(Cliente c)
        {
            if (c == null)
            {
                throw new Exception("Datos de cliente inválidos.");
            }

            if (string.IsNullOrWhiteSpace(c.Documento))
            {
                throw new Exception("DNI requerido.");
            }

            if (string.IsNullOrWhiteSpace(c.Nombre))
            {
                throw new Exception("Nombre requerido.");
            }

            if (string.IsNullOrWhiteSpace(c.Apellido))
            {
                throw new Exception("Apellido requerido.");
            }

            if (string.IsNullOrWhiteSpace(c.Email))
            {
                throw new Exception("Email requerido.");
            }

            if (!c.Email.Contains("@") || !c.Email.Contains("."))
            {
                throw new Exception("Email inválido.");
            }

            if (string.IsNullOrWhiteSpace(c.Direccion))
            {
                throw new Exception("Dirección requerida.");
            }

            if (string.IsNullOrWhiteSpace(c.Ciudad))
            {
                throw new Exception("Ciudad requerida.");
            }

            if (c.CodigoPostal <= 0 || string.IsNullOrWhiteSpace(c.ToString()))
            {
                throw new Exception("Código Postal inválido.");
            }
        }


       
        public Cliente RegistrarCliente(Cliente c)
        {
            ValidarClienteObligatorio(c);

            var existente = _clienteRepo.ObtenerPorDocumento(c.Documento);
            if (existente != null)
            {
                throw new Exception("El DNI ya está registrado. Use actualización.");
            }

            int id = _clienteRepo.Insertar(c);
            c.Id = id;
            return c;
        }

        public void ActualizarCliente(Cliente c)
        {
            if (c == null || c.Id <= 0)
            {
                throw new Exception("Cliente inválido para actualizar.");
            }

            ValidarClienteObligatorio(c);
            _clienteRepo.Actualizar(c);
        }

        public void ConfirmarParticipacion(string codigoVoucher, int idCliente, int idArticulo)
        {
            if (string.IsNullOrWhiteSpace(codigoVoucher))
            {
                throw new Exception("Código de voucher requerido.");
            }

            if (idCliente <= 0)
            {
                throw new Exception("Cliente inválido.");
            }

            if (idArticulo <= 0)
            {
                throw new Exception("Premio inválido.");
            }

            var v = ValidarVoucher(codigoVoucher);

            bool premioExiste = false;
            var premios = _articuloRepo.Listar();
            foreach (var a in premios)
            {
                if (a.Id == idArticulo)
                {
                    premioExiste = true;
                    break;
                }
            }

            if (!premioExiste)
            {
                throw new Exception("El premio seleccionado no existe.");
            }

            _voucherRepo.MarcarUsado(v.CodigoVoucher, idCliente, idArticulo);
        }

        public string BuscarNombreArticulo(int id)
        {
            try
            {
                return _articuloRepo.ObtenerNombrePorCodigo(id);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}

