using System;
using System.Web.UI.WebControls;
using PromoWeb.DAL;

namespace PromoWeb
{
    public partial class SeleccionPremio : System.Web.UI.Page
    {
        private readonly PromoRepository _repo = new PromoRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var codigo = Request.QueryString["code"];
            if (string.IsNullOrWhiteSpace(codigo))
            {
                Response.Redirect("Default.aspx");
                return;
            }

            bool usado;
            if (!_repo.TryValidarVoucher(codigo, out usado) || usado)
            {
                Response.Redirect("VoucherInvalido.aspx?code=" + Server.UrlEncode(codigo));
                return;
            }

            hfCodigo.Value = codigo;
            repPremios.DataSource = _repo.GetArticulos();
            repPremios.DataBind();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            int? selectedId = null;
            foreach (RepeaterItem it in repPremios.Items)
            {
                var rb = (RadioButton)it.FindControl("rbSel");
                if (rb != null && rb.Checked)
                {
                    var hf = (HiddenField)it.FindControl("hfId");
                    selectedId = int.Parse(hf.Value);
                    break;
                }
            }

            if (selectedId == null)
            {
                lblErr.Text = "Elegí un premio.";
                return;
            }
            Response.Redirect($"Registrar.aspx?code={Server.UrlEncode(hfCodigo.Value)}&artId={selectedId.Value}");
        }
    }
}