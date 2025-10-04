using System;
using Dominio;
using Business;

namespace PromoWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        
        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
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