<%@ Page Title="Elegí tu premio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SeleccionPremio.aspx.cs" Inherits="PromoWeb.SeleccionPremio" %>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
 
    <h2 class="titulos">Seleciona Un Premio</h2>
  <asp:HiddenField ID="hfCodigo" runat="server" />
  <asp:Repeater ID="repPremios" runat="server">
    <ItemTemplate>
      <div class="card mb-3">
        <div class="row g-0 align-items-center">
          <div class="col-md-3">
            <img class="img-fluid" alt='<%# Eval("Nombre") %>' src='<%# string.IsNullOrEmpty(Eval("PrimeraImagenUrl") as string) ? "img/noimage.png" : Eval("PrimeraImagenUrl") %>' />
          </div>
          <div class="col-md-7 p-3">
            <h5><%# Eval("Nombre") %></h5>
            <p><%# Eval("Descripcion") %></p>
          </div>
          <div class="col-md-2 p-3 text-center">
            <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("Id") %>' />
            <asp:RadioButton ID="rbSel" runat="server" GroupName="premio" />
          </div>
        </div>
      </div>
    </ItemTemplate>
  </asp:Repeater>
  <asp:Label ID="lblErr" runat="server" CssClass="text-danger d-block" />
  <asp:Button ID="btnNext" runat="server" CssClass="btn btn-primary" Text="Siguiente" OnClick="btnNext_Click" />
</asp:Content>