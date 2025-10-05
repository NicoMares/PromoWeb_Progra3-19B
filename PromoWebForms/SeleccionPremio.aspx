<%@ Page Title="Elegí tu premio" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="SeleccionPremio.aspx.cs" Inherits="PromoWeb.SeleccionPremio"
    ResponseEncoding="utf-8" Culture="es-AR" UICulture="es-AR" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <h2 class="titulos">Elegí tu premio</h2>

  <asp:HiddenField ID="hfCodigo" runat="server" />

  <asp:Repeater ID="repPremios" runat="server">
    <ItemTemplate>
      <div class="card premio-card mb-3">
        <div class="row g-0 align-items-center">

          <div class="col-md-4 p-3">
            <div id='car_<%# Eval("Id") %>' class="carousel slide" data-bs-ride="carousel">
              <div class="carousel-inner">
                <asp:Repeater ID="repImgs" runat="server" DataSource='<%# Eval("Imagenes") %>'>
                  <ItemTemplate>
                    <div class='carousel-item <%# Container.ItemIndex == 0 ? "active" : "" %>'>
                      <img class="d-block w-100 articulo-img" src='<%# Eval("ImagenUrl") %>' alt="Foto" />
                    </div>
                  </ItemTemplate>
                </asp:Repeater>
              </div>
              <button class="carousel-control-prev" type="button"
                      data-bs-target='#car_<%# Eval("Id") %>' data-bs-slide="prev">
                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                <span class="visually-hidden">Anterior</span>
              </button>
              <button class="carousel-control-next" type="button"
                      data-bs-target='#car_<%# Eval("Id") %>' data-bs-slide="next">
                <span class="carousel-control-next-icon" aria-hidden="true"></span>
                <span class="visually-hidden">Siguiente</span>
              </button>
            </div>
          </div>

          <div class="col-md-8 p-3">
            <h5 class="card-title mb-1"><%# Eval("Nombre") %></h5>
            <p class="card-text text-muted"><%# Eval("Descripcion") %></p>

            <div class="form-check mt-2">
              <input class="form-check-input" type="radio"
                     name="premio" value='<%# Eval("Id") %>' id='premio_<%# Eval("Id") %>' />
              <label class="form-check-label" for='premio_<%# Eval("Id") %>'>Elegir este premio</label>
            </div>
          </div>

        </div>
      </div>
    </ItemTemplate>
  </asp:Repeater>

  <asp:Label ID="lblErr" runat="server" CssClass="text-danger d-block" />
  <asp:Button ID="btnNext" runat="server" CssClass="btn btn-primary" Text="Siguiente" OnClick="btnNext_Click" />

  <script>
    document.addEventListener('change', function (e) {
      if (e.target && e.target.matches('input[name="premio"]')) {
        document.querySelectorAll('.premio-card').forEach(function (c) { c.classList.remove('is-selected'); });
        var card = e.target.closest('.premio-card');
        if (card) card.classList.add('is-selected');
      }
    });
  </script>
</asp:Content>
