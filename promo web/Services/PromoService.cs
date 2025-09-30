using Microsoft.EntityFrameworkCore;
using Promos.Web.Data;
using Promos.Web.Models;

namespace Promos.Web.Services
{
    public class PromoService : IPromoService
    {
        private readonly PromosDbContext _db;
        public PromoService(PromosDbContext db) => _db = db;

        public async Task<(bool ok, bool yaUsado)> ValidarVoucherAsync(string codigo)
        {
            var v = await _db.Vouchers.AsNoTracking().FirstOrDefaultAsync(x => x.CodigoVoucher == codigo);
            if (v is null) return (false, false);
            return (true, v.IdCliente != null);
        }

        public async Task<Step2Vm> BuildPremiosAsync(string codigo)
        {
            var articulos = await _db.ARTICULOS
                .Include(a => a.Imagenes)
                .OrderBy(a => a.Nombre)
                .Select(a => new Step2Vm.ArticuloCard
                {
                    Id = a.Id,
                    Nombre = a.Nombre ?? "",
                    Descripcion = a.Descripcion,
                    PrimeraImagenUrl = a.Imagenes.Select(i => i.ImagenUrl).FirstOrDefault()
                })
                .ToListAsync();

            return new Step2Vm
            {
                Codigo = codigo,
                SelectedArticuloId = null,
                Articulos = articulos
            };
        }

        public Task<Cliente?> BuscarClientePorDniAsync(string dni) =>
            _db.Clientes.FirstOrDefaultAsync(c => c.Documento == dni);

        public async Task<SuccessVm> RegistrarYCanjearAsync(Step3Vm vm)
        {
            // 1) Voucher válido y no usado
            var v = await _db.Vouchers.FirstOrDefaultAsync(x => x.CodigoVoucher == vm.Codigo);
            if (v is null || v.IdCliente != null) throw new InvalidOperationException("Voucher inválido o ya usado");

            // 2) Cliente: upsert por DNI
            var cli = await _db.Clientes.FirstOrDefaultAsync(c => c.Documento == vm.Documento);
            if (cli is null)
            {
                cli = new Cliente
                {
                    Documento = vm.Documento,
                    Nombre = vm.Nombre,
                    Apellido = vm.Apellido,
                    Email = vm.Email,
                    Direccion = vm.Direccion,
                    Ciudad = vm.Ciudad,
                    CP = vm.CP
                };
                _db.Clientes.Add(cli);
                await _db.SaveChangesAsync();
            }
            else
            {
                cli.Nombre = vm.Nombre;
                cli.Apellido = vm.Apellido;
                cli.Email = vm.Email;
                cli.Direccion = vm.Direccion;
                cli.Ciudad = vm.Ciudad;
                cli.CP = vm.CP;
                _db.Clientes.Update(cli);
                await _db.SaveChangesAsync();
            }

            // 3) Canje
            v.IdCliente = cli.Id;
            v.IdArticulo = vm.SelectedArticuloId;
            v.FechaCanje = DateOnly.FromDateTime(DateTime.Now);
            await _db.SaveChangesAsync();

            var premio = await _db.ARTICULOS
                .Where(a => a.Id == vm.SelectedArticuloId)
                .Select(a => a.Nombre ?? "")
                .FirstAsync();

            return new SuccessVm
            {
                Codigo = v.CodigoVoucher,
                Premio = premio,
                NombreCompleto = $"{cli.Nombre} {cli.Apellido}"
            };
        }
    }
}
