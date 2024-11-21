using AuthAPI903.Data;
using AuthAPI903.Dtos;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI903.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PagoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<PagoDto>>> ObtenerPagos()
        {
            var pagos = await _context.Pagos
                .Include(p => p.Suscripcion)
                .Select(p => new PagoDto
                {
                    Id = p.Id,
                    Cantidad = p.Cantidad,
                    FechaDePago = p.FechaDePago,
                    SuscripcionId = p.SuscripcionId
                })
                .ToListAsync();

            if (!pagos.Any())
                return NotFound("No se encontraron pagos.");

            return Ok(pagos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PagoDto>> ObtenerPagoPorId(int id)
        {
            var pago = await _context.Pagos
                .Include(p => p.Suscripcion)
                .Where(p => p.Id == id)
                .Select(p => new PagoDto
                {
                    Id = p.Id,
                    Cantidad = p.Cantidad,
                    FechaDePago = p.FechaDePago,
                    SuscripcionId = p.SuscripcionId
                })
                .FirstOrDefaultAsync();

            if (pago == null)
                return NotFound("Pago no encontrado.");

            return Ok(pago);
        }

        [HttpPost("registrar-pago")]
        public async Task<ActionResult> RegistrarPago([FromBody] RegistrarPagoDto pagoDto)
        {
            var pago = new Pago
            {
                Cantidad = pagoDto.Cantidad,
                FechaDePago = pagoDto.FechaDePago,
                SuscripcionId = pagoDto.SuscripcionId
            };

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pago registrado exitosamente", pagoId = pago.Id });
        }

        [HttpPut("{id}/actualizar")]
        public async Task<IActionResult> ActualizarPago(int id, [FromBody] ActualizarPagoDto pagoDto)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
                return NotFound("Pago no encontrado.");

            // Actualiza solo los campos que fueron proporcionados
            if (pagoDto.Cantidad.HasValue)
                pago.Cantidad = pagoDto.Cantidad.Value;

            if (pagoDto.FechaDePago.HasValue)
                pago.FechaDePago = pagoDto.FechaDePago.Value;

            _context.Entry(pago).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Ocurrió un problema al actualizar el pago.");
            }

            return Ok("Pago actualizado exitosamente.");
        }

        [HttpDelete("{id}/eliminar")]
        public async Task<IActionResult> EliminarPago(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
                return NotFound("Pago no encontrado.");

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();

            return Ok("Pago eliminado exitosamente.");
        }
    }
}
