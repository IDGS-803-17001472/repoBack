using AuthAPI903.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AuthAPI903.Data;

namespace AuthAPI903.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableroRendimientoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TableroRendimientoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<TableroRendimiento> Get()
        {
            // Obtener el primer tablero de rendimiento de la base de datos
            var tablero = _context.TableroRendimientos.FirstOrDefault();
            if (tablero == null)
            {
                return NotFound();
            }

            return Ok(tablero);
        }
    }
}