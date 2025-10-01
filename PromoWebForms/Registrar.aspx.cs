using System;
using PromoWeb.DAL;

namespace PromoWeb
{
    public partial class Registrar : System.Web.UI.Page
    {
        private readonly PromoRepository _repo = new PromoRepository();

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

            bool usado;
            if (!_repo.TryValidarVoucher(code, out usado) || usado)
            {
                Response.Redirect("VoucherInvalido.aspx?code=" + Server.UrlEncode(code));
                return;
            }

            hfCodigo.Value = code;
            hfArticuloId.Value = art;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            var dni = (txtDni.Text ?? "").Trim();
            if (string.IsNullOrEmpty(dni)) { lblErr.Text = "Ingresá DNI."; return; }

            var c = _repo.GetClienteByDni(dni);
            if (c != null)
            {
                txtNombre.Text = c.Nombre;
                txtApellido.Text = c.Apellido;
                txtEmail.Text = c.Email;
                txtDireccion.Text = c.Direccion;
                txtCiudad.Text = c.Ciudad;
                txtCP.Text = c.CP.ToString();
            }
            else
            {
                lblErr.Text = "No existe cliente con ese DNI. Completá los datos y se creará.";
            }
        }

        protected void btnParticipar_Click(object sender, EventArgs e)
        {
            lblErr.Text = "";
            if (string.IsNullOrWhiteSpace(txtDni.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtDireccion.Text) ||
                string.IsNullOrWhiteSpace(txtCiudad.Text) ||
                string.IsNullOrWhiteSpace(txtCP.Text))
            {
                lblErr.Text = "Completá todos los campos.";
                return;
            }

            int cp;
            if (!int.TryParse(txtCP.Text.Trim(), out cp))
            {
                lblErr.Text = "CP inválido.";
                return;
            }

            var cli = new ClienteDto
            {
                Documento = txtDni.Text.Trim(),
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Direccion = txtDireccion.Text.Trim(),
                Ciudad = txtCiudad.Text.Trim(),
                CP = cp
            };

            var clienteId = _repo.UpsertCliente(cli);
            var articuloId = int.Parse(hfArticuloId.Value);
            _repo.CanjearVoucher(hfCodigo.Value, clienteId, articuloId);

            var premio = _repo.GetArticuloNombre(articuloId);
            var nombreCompleto = (cli.Nombre + " " + cli.Apellido).Trim();

            Response.Redirect($"Exito.aspx?code={Server.UrlEncode(hfCodigo.Value)}&premio={Server.UrlEncode(premio)}&nombre={Server.UrlEncode(nombreCompleto)}");
        }
    }
}