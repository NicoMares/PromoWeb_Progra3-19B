<%@ Page Title="Éxito" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Exito.aspx.cs" Inherits="PromoWeb.Exito" %>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <div class="alert alert-success">
    <h4>¡Listo, <%= Server.HtmlEncode(Request.QueryString["nombre"] ?? "") %>!</h4>
    <p>Registramos tu participación con el código <strong><%= Server.HtmlEncode(Request.QueryString["code"] ?? "") %></strong> por el premio <strong><%= Server.HtmlEncode(Request.QueryString["premio"] ?? "") %></strong>.</p>
  </div>
  <a class="btn btn-secondary" href="Default.aspx">Volver al inicio</a>
</asp:Content>