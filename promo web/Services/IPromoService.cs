using Promos.Web.Data;
using Promos.Web.Models;

namespace Promos.Web.Services;

public interface IPromoService
{
    Task<(bool ok, bool yaUsado)> ValidarVoucherAsync(string codigo);
    Task<Step2Vm> BuildPremiosAsync(string codigo);
    Task<Cliente?> BuscarClientePorDniAsync(string dni);
    Task<SuccessVm> RegistrarYCanjearAsync(Step3Vm vm);
}
