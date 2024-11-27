using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using AuthAPI903.Data;

namespace AuthAPI903.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public IActionResult GetDashboardSummary()
        {
            // Número total de cotizaciones
            var totalCotizaciones = _context.Cotizaciones.Count();

            // Número total de suscripciones
            var totalSuscripciones = _context.Suscripciones.Count();

            // Número de suscripciones agrupadas por tipo de plan
            var suscripcionesPorPlan = _context.Suscripciones
                .GroupBy(s => s.PlanId)
                .Select(group => new
                {
                    PlanId = group.Key,
                    PlanNombre = _context.PlanesSuscripcion
                        .Where(p => p.Id == group.Key)
                        .Select(p => p.Nombre)
                        .FirstOrDefault(),
                    Cantidad = group.Count()
                })
                .ToList();

            var resultado = new
            {
                TotalCotizaciones = totalCotizaciones,
                TotalSuscripciones = totalSuscripciones,
                SuscripcionesPorPlan = suscripcionesPorPlan
            };

            return Ok(resultado);
        }
    }
}
