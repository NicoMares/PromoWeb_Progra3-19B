using Business;
using Dominio;
using System;
using System.Data.SqlClient;

namespace PromoWeb
{
    public partial class Registrar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var code = Request.QueryString["code"];
            var art = Request.QueryString["artId"];
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(art))
            {
                Response.Redirect("Default.aspx");
                return;
            }
                   
            hfCodigo.Value = code;
            hfArticuloId.Value = art;
            
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            PromoBusiness promoBsns = new PromoBusiness();
            try
            {
                Cliente cliente = promoBsns.BuscarCliente(txtDni.Text.Trim());
                if (cliente != null)
                {
                    txtNombre.Text = cliente.Nombre;
                    txtApellido.Text = cliente.Apellido;
                    txtEmail.Text = cliente.Email;
                    txtDireccion.Text = cliente.Direccion;
                    txtCiudad.Text = cliente.Ciudad;
                    txtCP.Text = cliente.CodigoPostal.ToString();
                }
                else
                {
                    txtNombre.Text = "";
                    txtApellido.Text = "";
                    txtEmail.Text = "";
                    txtDireccion.Text = "";
                    txtCiudad.Text = "";
                    txtCP.Text = "";
                    lblErr.Text = "No existe cliente con ese DNI. Completá los datos y se creará.";
                }
            }
            catch (Exception ex)
            {

                lblErr.Text = ex.Message;
            }

        }

        protected void btnParticipar_Click(object sender, EventArgs e)
        {

            PromoBusiness promoBsns = new PromoBusiness();

           int cp;
           if (!int.TryParse(txtCP.Text.Trim(), out cp))
               {
                   lblErr.Text = "CP inválido.";
                   return;
               }
            try
            {

                Cliente cliente = new Cliente
                {
                    Documento = txtDni.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim(),
                    Ciudad = txtCiudad.Text.Trim(),
                    CodigoPostal = cp
                
                };
                promoBsns.RegistrarCliente(cliente);
                string codigo = (string)Session["VoucherCodigo"];
                int idCliente = cliente.Id;
                
                string idArticulo = Request.QueryString["artId"];
                string premio;
                if (int.TryParse(idArticulo, out int artId))
                {
                    promoBsns.ConfirmarParticipacion(codigo,idCliente,artId);
                    premio = promoBsns.BuscarNombreArticulo(artId);                    
                }
                else
                {
                    throw new Exception("Hubo un error inesperado");
                }
                string nombreCompleto = cliente.Nombre;
                Response.Redirect($"Exito.aspx?code={Server.UrlEncode(hfCodigo.Value)}&premio={Server.UrlEncode(premio)}&nombre={Server.UrlEncode(nombreCompleto)}");

            }
            catch (Exception ex)
            {

                lblErr.Text = ex.Message;
            }




        }
    }
}