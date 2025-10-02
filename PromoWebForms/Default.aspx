<%@ Page Title="Ingresar código" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PromoWeb._Default" %>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
 
    <div class="codigo-voucher">
    
  
    <label class="form-label">Ingresá el código de tu voucher</label>
    <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCodigo" ErrorMessage="Requerido" CssClass="text-danger" />
  </div>

     
  <asp:Button ID="btnSiguiente" runat="server" CssClass="btn btn-primary" Text="Siguiente" OnClick="btnSiguiente_Click" />
  <asp:Label ID="lblError" runat="server" CssClass="text-danger d-block mt-2" />
</asp:Content>