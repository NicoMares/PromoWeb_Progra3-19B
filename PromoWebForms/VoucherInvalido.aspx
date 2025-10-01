<%@ Page Title="Código inválido" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VoucherInvalido.aspx.cs" Inherits="PromoWeb.VoucherInvalido" %>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <h3>Código inválido o ya utilizado</h3>
  <p>El código <strong><%= Server.HtmlEncode(Request.QueryString["code"] ?? "") %></strong> no puede usarse.</p>
  <a class="btn btn-secondary" href="Default.aspx">Volver al inicio</a>
</asp:Content>