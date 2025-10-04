using System;
using PromoWeb.DAL;
using Dominio;
using Business;

namespace PromoWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        private readonly PromoRepository _repo = new PromoRepository();

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            //var codigo = (txtCodigo.Text ?? "").Trim();
            //if (string.IsNullOrEmpty(codigo))
            //{
            //    lblError.Text = "Ingresá el código a Canjear.";
            //    return;
            //}

            //bool usado;
            //var ok = _repo.TryValidarVoucher(codigo, out usado);
            //if (!ok || usado)
            //{
            //    Response.Redirect("VoucherInvalido.aspx?code=" + Server.UrlEncode(codigo));
            //    return;
            //}
            //Response.Redirect("SeleccionPremio.aspx?code=" + Server.UrlEncode(codigo));


            PromoBusiness promoBsns = new PromoBusiness();

            try
            {
                var voucher = promoBsns.ValidarVoucher(txtCodigo.Text.Trim());

                Session["VoucherCodigo"] = voucher.CodigoVoucher;

                Response.Redirect("SeleccionPremio.aspx");

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }

        }
    }
}