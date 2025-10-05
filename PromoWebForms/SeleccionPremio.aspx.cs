using System;
using Business;   // tu capa de negocio
// using Dominio; // si lo necesitás

namespace PromoWeb
{
    public partial class SeleccionPremio : System.Web.UI.Page
    {
        private readonly PromoBusiness _promoBsns = new PromoBusiness();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            // Tomamos el código desde la sesión (como venías haciéndolo)
            var codigo = (string)Session["VoucherCodigo"];

            // Si por alguna razón no está, podés fallback a querystring o volver al inicio
            if (string.IsNullOrWhiteSpace(codigo))
            {
                // var qs = Request.QueryString["code"]; // fallback opcional
                // if (string.IsNullOrWhiteSpace(qs)) {
                Response.Redirect("Default.aspx");
                return;
                // }
                // codigo = qs;
            }

            hfCodigo.Value = codigo;

            // IMPORTANTE:
            // Asegurate de que ListarPremios() devuelva Articulos con Imagenes cargadas.
            // (Abajo te dejo cómo ajustar PromoBusiness para eso)
            repPremios.DataSource = _promoBsns.ListarPremios();
            repPremios.DataBind();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            // Toma el valor del radio seleccionado (name="premio" en la vista)
            var selected = Request.Form["premio"];
            if (string.IsNullOrEmpty(selected))
            {
                lblErr.Text = "Elegí un premio.";
                return;
            }

            Response.Redirect($"Registrar.aspx?code={Server.UrlEncode(hfCodigo.Value)}&artId={selected}");
        }
    }
}
