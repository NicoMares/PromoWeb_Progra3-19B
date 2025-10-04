<%@ Page Title="Registrar datos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registrar.aspx.cs" Inherits="PromoWeb.Registrar" %>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
 <h2 class="titulos">Contactos</h2>
  <asp:HiddenField ID="hfCodigo" runat="server" />
  <asp:HiddenField ID="hfArticuloId" runat="server" />

  <div class="row g-3">
    <div class="col-md-4">
      <label class="form-label">DNI</label>
      <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" />
    </div>
    <div class="col-md-2 d-flex align-items-end">
      <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-outline-secondary" Text="Buscar" OnClick="btnBuscar_Click" />
    </div>
    <div class="col-md-4">
      <label class="form-label">Nombre</label>
      <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
    </div>
    <div class="col-md-4">
      <label class="form-label">Apellido</label>
      <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
    </div>
    <div class="col-md-6">
      <label class="form-label">Email</label>
      <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
    </div>
    <div class="col-md-6">
      <label class="form-label">Dirección</label>
      <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" />
    </div>
    <div class="col-md-6">
      <label class="form-label">Ciudad</label>
      <asp:TextBox ID="txtCiudad" runat="server" CssClass="form-control" />
    </div>
    <div class="col-md-2">
      <label class="form-label">CP</label>
      <asp:TextBox ID="txtCP" runat="server" CssClass="form-control" />
    </div>
  </div>

  <div class="form-check mt-3">
    <input class="form-check-input" type="checkbox" id="tyc" required />
    <label class="form-check-label" for="tyc">Acepto los términos y condiciones.</label>
  </div>

  <asp:Label ID="lblErr" runat="server" CssClass="text-danger d-block mt-2" />
  <asp:Button ID="btnParticipar" runat="server" CssClass="btn btn-success mt-2" Text="¡Participar!" OnClick="btnParticipar_Click" />
</asp:Content>