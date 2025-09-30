using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Promos.Web.Data;

namespace Promos.Web.Controllers;

[Route("api/clientes")]
[ApiController]
public class ClientesApiController : ControllerBase
{
    private readonly PromosDbContext _db;
    public ClientesApiController(PromosDbContext db) => _db = db;

    [HttpGet("by-dni/{dni}")]
    public async Task<IActionResult> GetByDni(string dni)
    {
        var c = await _db.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.Documento == dni);
        if (c is null) return Ok(null);
        return Ok(new { c.Nombre, c.Apellido, c.Email, c.Direccion, c.Ciudad, c.CP });
    }
}
