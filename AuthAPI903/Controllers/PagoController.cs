using AuthAPI903.Data;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthAPI903.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PagoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> RegistrarPago([FromBody] Pago pago)
        {
            var pagoNuevo = new Pago
            {
                Id = pago.Id,
                Cantidad = pago.Cantidad,
                FechaDePago = DateTime.Now
            };

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pago registrado exitosamente", pagoId = pago.Id });
        }

        [HttpGet("suscripcion/{idSuscripcion}")]
        public async Task<ActionResult<List<Pago>>> ObtenerPagosPorSuscripcion(int idSuscripcion)
        {
            var pagos = await _context.Pagos
                .Where(p => p.Id == idSuscripcion)
                .ToListAsync();

            if (pagos.Count == 0)
                return NotFound("No se encontraron pagos para esta suscripción.");

            return Ok(pagos);
        }
    }
}
