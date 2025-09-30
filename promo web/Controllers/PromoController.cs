using Microsoft.AspNetCore.Mvc;
using Promos.Web.Models;
using Promos.Web.Services;

namespace Promos.Web.Controllers
{
    public class PromoController : Controller
    {
        private readonly IPromoService _svc;
        public PromoController(IPromoService svc) => _svc = svc;

        // Paso 1: ingresar código
        [HttpGet]
        public IActionResult Index() => View();

        // ⚠️ Binding explícito al campo "codigo" del form
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                ModelState.AddModelError("", "Ingresá el código de tu voucher.");
                return View();
            }

            var (ok, usado) = await _svc.ValidarVoucherAsync(codigo.Trim());
            if (!ok || usado)
                return RedirectToAction(nameof(VoucherInvalido), new { code = codigo });

            return RedirectToAction(nameof(SeleccionPremio), new { code = codigo });
        }

        // Pantalla error
        [HttpGet]
        public IActionResult VoucherInvalido(string code) => View(model: code);

        // Paso 2: listar y elegir premio
        [HttpGet]
        public async Task<IActionResult> SeleccionPremio(string code)
        {
            var (ok, usado) = await _svc.ValidarVoucherAsync(code);
            if (!ok || usado) return RedirectToAction(nameof(VoucherInvalido), new { code });
            var vm = await _svc.BuildPremiosAsync(code);
            return View(vm);
        }

        [HttpPost]
        public IActionResult SeleccionPremio(Step2Vm vm)
        {
            if (vm.SelectedArticuloId is null)
            {
                ModelState.AddModelError(nameof(vm.SelectedArticuloId), "Elegí un premio");
                return View(vm);
            }
            return RedirectToAction(nameof(Registrar), new { code = vm.Codigo, artId = vm.SelectedArticuloId.Value });
        }

        // Paso 3: datos personales
        [HttpGet]
        public IActionResult Registrar(string code, int artId) =>
            View(new Step3Vm { Codigo = code, SelectedArticuloId = artId });

        [HttpPost]
        public async Task<IActionResult> Registrar(Step3Vm vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var ok = await _svc.ValidarVoucherAsync(vm.Codigo);
            if (!ok.ok || ok.yaUsado) return RedirectToAction(nameof(VoucherInvalido), new { code = vm.Codigo });
            var success = await _svc.RegistrarYCanjearAsync(vm);
            return RedirectToAction(nameof(Exito), success);
        }

        // Éxito
        [HttpGet]
        public IActionResult Exito(SuccessVm vm) => View(vm);
    }
}
